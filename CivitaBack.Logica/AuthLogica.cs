using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Logica.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CivitaBack.Logica
{
    public interface IAuthLogica
    {
        Task<Usuario> RegistrarUsuarioAsync(string nombreUsuario, string mail, string password);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<Usuario> ObtenerPorId(int id);
        Task<Usuario> ObtenerUsuarioPorNombre(string nombre);

    }
    public class AuthLogica : IAuthLogica
    {
        private readonly IUsuarioRepositorio _repositorioUsuario;
        private readonly IConfiguration _configuration;

        public AuthLogica(IUsuarioRepositorio repositorioUsuario, IConfiguration configuration)
        {
            _repositorioUsuario = repositorioUsuario;
            _configuration = configuration;

        }

        public Task<Usuario> ObtenerPorId(int id)
        {
            Task<Usuario> usuario = this._repositorioUsuario.ObtenerPorId(id);
            if (usuario == null) throw new Exception("No se encontró el usuario");
            return usuario;
        }

        public async Task<Usuario> ObtenerUsuarioPorNombre(string nombre)
        {
            Usuario usuario = await _repositorioUsuario.ObtenerUsuarioPorNombre(nombre);
            if (usuario == null) throw new Exception("No se encontró el usuario");
            return usuario;
        }

        public async Task<Usuario> RegistrarUsuarioAsync(string nombreUsuario, string mail, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
                throw new ValidacionRegistroException("Todos los campos son obligatorios.");

            if (await _repositorioUsuario.ObtenerUsuarioPorMail(mail) != null)
                throw new ValidacionRegistroException("El correo ya está en uso.");

            if (await ObtenerUsuarioPorNombre(nombreUsuario) != null)
                throw new ValidacionRegistroException("El nombre de usuario ya está en uso.");

            if (!Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ValidacionRegistroException("El correo no tiene un formato válido.");

            if (password.Length < 4)
                throw new ValidacionRegistroException("La contraseña debe tener al menos 4 caracteres.");

            var hash = PasswordHelper.HashPassword(password);

            var usuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                Mail = mail,
                HashDeContrasena = hash
            };

            return await _repositorioUsuario.CrearUsuario(usuario);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _repositorioUsuario.ObtenerUsuarioPorMail(request.Mail);

            if (usuario == null || !PasswordHelper.VerifyPassword(request.Password, usuario.HashDeContrasena))
                throw new AutenticacionException("Usuario o contraseña incorrectos.");

            var token = GenerarToken(usuario);

            return new LoginResponse
            {
                Token = token,
                NombreUsuario = usuario.NombreUsuario,
                Mail = usuario.Mail,
                IdUsuario = usuario.Id
            };
        }

        private string GenerarToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario.Mail)
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
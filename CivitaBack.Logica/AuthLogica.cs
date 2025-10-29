using CivitaBack.Domain.Entities;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Logica.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace CivitaBack.Logica
{
    public interface IAuthLogica
    {
        Task<RegistroResponse> RegistrarUsuarioAsync(string nombreUsuario, string mail, string password);
        Task<LoginResponse> LoginAsync(LoginRequest request);

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

        public async Task<RegistroResponse> RegistrarUsuarioAsync(string nombreUsuario, string mail, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
                throw new ValidacionRegistroException("Todos los campos son obligatorios.");

            if (await _repositorioUsuario.ObtenerUsuarioPorMail(mail) != null)
                throw new ValidacionRegistroException("El correo ya está en uso.");

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

            var usuarioCreado = await _repositorioUsuario.CrearUsuario(usuario);
            
            if (usuarioCreado == null)
                throw new ValidacionRegistroException("Error al crear el usuario.");

            return new RegistroResponse
            {
                Id = usuarioCreado.Id,
                NombreUsuario = usuarioCreado.NombreUsuario,
                Mail = usuarioCreado.Mail
            };

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
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
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
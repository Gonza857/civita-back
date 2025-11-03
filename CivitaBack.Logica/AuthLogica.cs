using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Logica.Helpers;
using CivitaBack.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CivitaBack.Domain.Interfaces.Logica;

namespace CivitaBack.Logica
{
    public class AuthLogica : IAuthLogica
    {
        private readonly IUsuarioRepositorio _repositorioUsuario;
        private readonly IConfiguration _configuration;
        private readonly IUnidadDeTrabajo _uow;

        public AuthLogica(IUsuarioRepositorio repositorioUsuario, IConfiguration configuration, IUnidadDeTrabajo uow)
        {
            _repositorioUsuario = repositorioUsuario;
            _configuration = configuration;
            _uow = uow;
        }

        public async Task<Usuario> CrearUsuario(string nombreUsuario, string mail, string password)
        {
            await this.ValidarExistenciaCorreo(mail);
            
            var usuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                Mail = mail,
                HashDeContrasena = PasswordHelper.HashPassword(password)
            };

            return usuario;
        }

        public async Task<string> IniciarSesion(string mail, string contrasena)
        {
            this.ValidarUsuarioIniciarSesion(mail, contrasena);
            Usuario? usuario = await _repositorioUsuario.ObtenerUsuarioPorMail(mail);
            this.ValidarUsuarioAndContrasena(contrasena, usuario);
            return this.GenerarToken(usuario!);
        }

        private void ValidarUsuarioIniciarSesion(string mail, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(mail)) 
                throw new AutenticacionException("El nombre de usuario no puede estar vacio");
            if (string.IsNullOrWhiteSpace(contrasena)) 
                throw new AutenticacionException("La contrasena no puede estar vacia");
        }

        private void ValidarUsuarioAndContrasena(string inputContrasena, Usuario? usuario)
        {
            if (usuario == null)
                throw new AutenticacionException("Usuario o contraseña incorrectos.");
            
            if (!PasswordHelper.VerifyPassword(inputContrasena, usuario.HashDeContrasena!))
                throw new AutenticacionException("Usuario o contraseña incorrectos.");
        }

        private async Task ValidarExistenciaCorreo(string correo)
        {
            if (await _repositorioUsuario.ObtenerUsuarioPorMail(correo) != null)
                throw new ValidacionRegistroException("El correo ya está en uso.");
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
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Logica.Helpers;
using CivitaBack.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        private readonly IUnidadDeTrabajo _uow;

        public AuthLogica(IUsuarioRepositorio repositorioUsuario, IConfiguration configuration, IUnidadDeTrabajo uow)
        {
            _repositorioUsuario = repositorioUsuario;
            _configuration = configuration;
            _uow = uow;
        }

        public async Task<RegistroResponse> RegistrarUsuarioAsync(string nombreUsuario, string mail, string password)
        {
            if (await _repositorioUsuario.ObtenerUsuarioPorMail(mail) != null)
                throw new ValidacionRegistroException("El correo ya está en uso.");

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

            await _uow.CommitAsync();

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
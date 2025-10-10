using CivitaBack.Data.BO;
using CivitaBack.Data.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Logica
{
    public interface IAuthLogica
    {
        Task<Usuario> RegistrarUsuarioAsync(string nombreUsuario, string mail, string password);
    }
    public class AuthLogica : IAuthLogica
    {
        private readonly IRepositorioUsuario _repositorioUsuario;

        public AuthLogica(IRepositorioUsuario repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        public async Task<Usuario> RegistrarUsuarioAsync(string nombreUsuario, string mail, string password)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Todos los campos son obligatorios.");

            if (await _repositorioUsuario.obtenerUsuarioPorMail(mail) != null)
                throw new ArgumentException("El correo ya está en uso.");

            if (await _repositorioUsuario.obtenerUsuarioPorNombre(nombreUsuario) != null)
                throw new ArgumentException("El nombre de usuario ya está en uso.");

            // Hash de la contraseña
            var hash = PasswordHelper.HashPassword(password);

            var usuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                Mail = mail,
                HashDeContrasena = hash
            };

            return await _repositorioUsuario.CrearUsuario(usuario);
        }
    }
}

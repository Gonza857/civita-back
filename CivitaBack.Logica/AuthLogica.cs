using CivitaBack.Data.BO;
using CivitaBack.Data.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Todos los campos son obligatorios.");

            if (await _repositorioUsuario.obtenerUsuarioPorMail(mail) != null)
                throw new ArgumentException("El correo ya está en uso.");

            if (await _repositorioUsuario.obtenerUsuarioPorNombre(nombreUsuario) != null)
                throw new ArgumentException("El nombre de usuario ya está en uso.");

            if (!Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("El correo no tiene un formato válido.");

            if (password.Length < 4)
                throw new ArgumentException("La contraseña debe tener al menos 4 caracteres.");

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

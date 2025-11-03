using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IAuthLogica
{
    Task<Usuario> CrearUsuario(string nombreUsuario, string mail, string contrasena);
    Task<string> IniciarSesion(string mail, string contrasena);
}
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IAuthLogica
{
    Task<Usuario> CrearUsuario(string nombreUsuario, string mail, string contrasena);
    Task<Usuario> CrearUsuarioInicial(string nombreUsuario, Usuario? usuario);
    Task<string> IniciarSesion(string mail, string contrasena);
    Task<Usuario> IniciarSesion(string usuario);
    public string GenerarToken(Usuario usuario);

}
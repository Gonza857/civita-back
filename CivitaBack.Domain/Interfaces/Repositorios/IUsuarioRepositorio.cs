using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario> ObtenerUsuarioPorMail(string mail);
        Task<Usuario?> ObtenerUsuarioPorNombre(string nombreUsuario);
        Task<Usuario> CrearUsuario(Usuario usuario);

        Task<Usuario> ObtenerPorId(int id);
        Task<List<Usuario>> ObtenerTodosLosUsuarios();

    }
}

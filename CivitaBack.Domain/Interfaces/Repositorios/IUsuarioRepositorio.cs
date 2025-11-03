using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
        Task<Usuario?> ObtenerUsuarioPorMail(string mail);
        Task<Usuario?> ObtenerUsuarioPorNombre(string nombreUsuario);
        Task<Usuario> CrearUsuario(Usuario usuario);

    }
}

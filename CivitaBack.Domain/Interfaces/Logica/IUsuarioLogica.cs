using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface IUsuarioLogica
    {
        Task<Usuario> ObtenerPorId(int id);
        Task<Usuario> ObtenerPorCorreo(string correo);
        Task<Usuario?> ObtenerUsuarioPorNombre(string nombre);

    }
}

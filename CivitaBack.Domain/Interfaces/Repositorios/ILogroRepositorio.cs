using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ILogroRepositorio
    {
        Task<bool> ExisteLogroEnCumplidos(int idLogro);
        Task<Logro?> ObtenerPorId(int id);
        Task<List<Logro>> ObtenerTodos();
    }
}

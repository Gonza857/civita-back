using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ITipsRepositorio : IRepositorioBase<Tip>
    {
        Task<List<Tip>> ObtenerMsjPorIdTipo(int idTipo);
    }
}

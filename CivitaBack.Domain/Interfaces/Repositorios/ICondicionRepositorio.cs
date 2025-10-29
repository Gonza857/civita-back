using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ICondicionRepositorio : IRepositorioBase<Condicion>
    {
        Task<List<Condicion>> ObtenerTodasRecompensas();
    }
}

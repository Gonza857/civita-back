using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ITipsRepositorio : IRepositorioBase<Tip>
    {
        Task<List<Tip>> ObtenerMsjPorIdTipo(int idTipo, bool ascendente = true);
    }
}

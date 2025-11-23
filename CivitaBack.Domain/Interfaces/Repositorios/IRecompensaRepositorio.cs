using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios;

public interface IRecompensaRepositorio : IRepositorioBase<Recompensa>
{
    Task<List<Recompensa>> ObtenerVariosPorIds(List<int> ids);
}
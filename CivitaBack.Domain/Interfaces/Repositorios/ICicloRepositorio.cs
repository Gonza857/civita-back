using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface ICicloRepositorio
    {
        Task GuardarCambiosAsync();
        Task<List<Partida>> ObtenerPartidasConEstructuras();
    }
}

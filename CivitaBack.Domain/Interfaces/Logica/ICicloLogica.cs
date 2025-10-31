using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface ICicloLogica
    {
        Task<List<Partida>> EjecutarCicloAsync();
    }
}

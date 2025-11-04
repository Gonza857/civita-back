using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface ICicloLogica
    {
        Task<List<Partida>> EjecutarCicloAsync();
    }
}

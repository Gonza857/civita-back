using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IEventoRepositorio
    {
        Task<EventoMaestro?> ObtenerEventoMaestroAsync();
        Task CrearEventoAsync(Evento evento);
        Task<Evento?> ObtenerEventoConPartidaAsync(int eventoId);
    }
}

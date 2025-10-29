using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IEventoRepositorio
    {
        Task<EventoMaestro?> ObtenerEventoMaestroAsync();
        Task CrearEventoAsync(Evento evento);
        Task<Evento?> ObtenerEventoConPartidaAsync(int eventoId);
        Task GuardarCambiosAsync();
    }
}

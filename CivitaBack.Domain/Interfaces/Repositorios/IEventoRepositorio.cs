using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IEventoRepositorio : IRepositorioBase<Evento>
    {
        Task<EventoMaestro?> ObtenerEventoMaestroAsync(int idMaestro);
        Task<Evento> CrearEventoAsync(Evento evento);
        Task<Evento?> ObtenerEventoConPartidaAsync(int eventoId);
        Task<Evento> ObtenerPorId(int eventoId);
        Task<bool> ExisteTipEnviadoAsync(int partidaId, int eventoMaestroId);
    }
}

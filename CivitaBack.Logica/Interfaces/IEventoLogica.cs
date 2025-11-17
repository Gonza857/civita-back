using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Logica.Interfaces
{
    public interface IEventoLogica
    {
        Task<EventoDisparadoDTO> DispararEventoAsync(int idPartida);
        Task<EventoResueltoDTO> ResolverEventoPreguntaAsync(int eventoId, string respuesta);
        Task DispararTipContaminacionAsync(Partida partida);
    }
}

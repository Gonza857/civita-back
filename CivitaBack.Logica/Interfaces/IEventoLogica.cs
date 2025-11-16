using CivitaBack.Data.DTO;

namespace CivitaBack.Logica.Interfaces
{
    public interface IEventoLogica
    {
        Task<EventoDisparadoDTO> DispararEventoAsync(int idPartida);
        Task<EventoResueltoDTO> ResolverEventoPreguntaAsync(int eventoId, string respuesta);

    }
}

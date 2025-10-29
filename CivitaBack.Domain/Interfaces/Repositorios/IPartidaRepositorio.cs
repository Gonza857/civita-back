using CivitaBack.Domain.Entities;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IPartidaRepositorio
    {
        Task GuardarCambios();

        Task<Partida?> ObtenerPorUsuarioId(int IdUsuario);
        Task<Partida> CrearPartida(int idUsuario);
        Task<List<Partida>> ObtenerPartidas();
        void Guardar(Partida partida);

        Task<bool> ActualizarMapaAsync(Partida partida);

        Task ActualizarEstructurasMapaAsync(int partidaId, List<EstructuraMapa> estructuras);
        Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId);
        Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId);
        Task<List<EstructuraMapa>> ObtenerEstructurasDeUnMapa(int partidaId);
    }
}

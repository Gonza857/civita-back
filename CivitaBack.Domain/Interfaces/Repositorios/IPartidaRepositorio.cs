using CivitaBack.Domain.Entidades;

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
        
        Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId);
        Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId);
        Task<List<EstructuraMapa>> ObtenerEstructurasDeUnMapa(int partidaId);
    }
}

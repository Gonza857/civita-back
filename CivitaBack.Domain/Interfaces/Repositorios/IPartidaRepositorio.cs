using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IPartidaRepositorio : IRepositorioBase<Partida>
    {
        Task<Partida?> ObtenerPorUsuarioId(int idUsuario);
        Task<Partida?> ObtenerPorUsuarioCorreo(string correo);
        Task<Partida> CrearPartida(int idUsuario);
        Task<bool> ActualizarMapaAsync(Partida partida);
        
        Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId);
        Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId);
        Task<List<EstructuraMapa>> ObtenerEstructurasDeUnMapa(int partidaId);

        Task<List<Partida>> ObtenerTodasConEstructurasYRecursosAsync();

    }
}

using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Repositorios
{
    public interface IPartidaRepositorio : IRepositorioBase<Partida>
    {
        Task<Partida?> ObtenerPorUsuarioId(int idUsuario);
        Task<Partida?> ObtenerPorUsuarioCorreo(string correo);
        Task<Partida> CrearPartida(int idUsuario);
        Task<bool> ActualizarMapaAsync(Partida partida);

        void SincronizarCambios(Partida partidaDominio);

        Task<Partida?> ObtenerPorIdTrackeada(int idPartida);
        Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId);

        Task<Partida?> ObtenerPartidaConEstructuras(int partidaId);
        Task<List<EstructuraMapa>> ObtenerEstructurasDeUnMapa(int partidaId);

        Task<List<Partida>> ObtenerTodasConEstructurasYRecursosAsync();

    }
}

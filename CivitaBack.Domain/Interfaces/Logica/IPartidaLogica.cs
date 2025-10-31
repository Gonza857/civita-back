using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IPartidaLogica
{
    Task<Partida> ObtenerPorUsuarioId(int IdUsuario);
    Task<Partida> CrearPartida(int idUsuario);
    Task Actualizar(Partida partida, Usuario usuario);
    Task<List<Partida>> ObtenerPartidas();
    Task<Partida?> ObtenerPartidaPorIdInterno(int idUsuario);

    // 🆕 Métodos de mapa
    Task ActualizarMapaDePartidaAsync(int partidaId, string jsonMapa, List<EstructuraMapa>? estructuras);
    
    Task ReclamarLogros(Partida partida, List<Logro> logros);
    Task<Partida?> ObtenerMapaAsync(int partidaId);
}
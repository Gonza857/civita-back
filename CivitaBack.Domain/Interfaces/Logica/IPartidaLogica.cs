using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IPartidaLogica
{
    Task<Partida> ObtenerPorUsuarioId(int idUsuario);
    Task<Partida> CrearPartida(int idUsuario);
    Task Actualizar(Partida partida, Usuario usuario);
    Task<List<Partida>> ObtenerPartidas();
    Task<Partida?> ObtenerPartidaPorIdInterno(int idUsuario);
    
    Task ReclamarLogros(Partida partida, List<Logro> logros);    
    Task<Partida> ObtenerPorId(int idPartida);

    Task<Partida> ObtenerPartidaParaLogin(int idUsuario);
}
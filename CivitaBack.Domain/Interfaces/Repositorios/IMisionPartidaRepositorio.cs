using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;

namespace CivitaBack.Domain.Interfaces.Repositorios;

public interface IMisionPartidaRepositorio : IRepositorioBase<MisionPartida>
{
    Task AgregarMisionesPartida(List<Mision> misiones, Partida partida);
    Task<List<MisionPartida>> ObtenerMisionesPartida(int idPartida);
    Task<MisionPartida> ObtenerUnaMisionDePartida(int idPartida, int idMision);

    Task<Mision> ObtenerMisionPartidaPorId(int idPartida, int idMision);
    Task<List<MisionPartida>> Listado();
    Task<List<MisionPartida>> ListadoPorTipo(TipoMision tipoMision);
    
    Task<List<Mision>> ObtenerMisionesDia(int idUsuario);
    Task<List<Mision>> ObtenerMisionesSemana(int idUsuario);
    Task<List<Mision>> ObtenerMisionesMes(int idUsuario);
}
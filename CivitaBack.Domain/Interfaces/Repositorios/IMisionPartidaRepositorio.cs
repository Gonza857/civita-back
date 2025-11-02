using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;

namespace CivitaBack.Domain.Interfaces.Repositorios;

public interface IMisionPartidaRepositorio : IRepositorioBase<MisionPartida>
{
    Task AgregarMisionesPartida(List<Mision> misiones, Partida partida);
    Task<List<MisionPartida>> ObtenerMisionesPartida(int idPartida);
    Task<List<MisionPartida>> Listado();
    Task<List<MisionPartida>> ListadoPorTipo(TipoMision tipoMision);
}
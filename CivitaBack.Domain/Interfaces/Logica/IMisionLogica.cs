using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IMisionLogica
{
    Task ResetMisiones(TipoMision tipoMision);
    Task<List<Mision>> Listado();
    Task Crear(Mision mision);
    Task Actualizar(Mision mision, int idMision);
    Task<Mision> ObtenerPorId(int idMision);
    Task<List<Mision>> ObtenerMisionesActivasParaPartida(Partida partida);
    Task AsignarMisiones(Partida partida);

    Task<List<Mision>> ObtenerMisionesDisponibles();
}
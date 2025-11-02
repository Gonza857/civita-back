using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IMisionLogica
{
    Task<List<Mision>> ObtenerMisionesDia(int idUsuario);
    Task<List<Mision>> ObtenerMisionesSemana(int idUsuario);
    Task<List<Mision>> ObtenerMisionesMes(int idUsuario);
    
    Task ResetMisiones(TipoMision tipoMision);
    
    Task<List<Mision>> Listado();
    Task Crear(Mision mision);
    Task Actualizar(Mision mision, int idMision);
    Task<Mision> ObtenerPorId(int idMision);
    Task<List<Mision>> ObtenerMisionesActivas(Partida? partida);

    Task AsignarMisiones(Partida? partida);
}
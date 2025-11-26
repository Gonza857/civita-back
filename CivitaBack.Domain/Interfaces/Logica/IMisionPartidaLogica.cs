using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IMisionPartidaLogica
{
    Task<Mision> ObtenerMisionPartidaPorId(int idPartida, int idMision);
    
    Task<List<Mision>> ObtenerMisionesDia(int idUsuario);
    Task<List<Mision>> ObtenerMisionesSemana(int idUsuario);
    Task<List<Mision>> ObtenerMisionesMes(int idUsuario);
    
    Task AsignarMisiones(List<Mision> misiones, Partida partida);
    Task MarcarMisionCompletada(Mision mision, Partida partida);
    
    Task<List<MisionPartida>> ObtenerMisionesActivasParaPartida(Partida partida);

    List<MisionPartida> ProcesarMisionesPartida(List<MisionPartida> reclamables, List<MisionPartida> noReclamables);


}
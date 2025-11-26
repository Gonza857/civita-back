using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IMisionPartidaLogica
{
/// <summary>
    /// Obtiene una Misión específica asociada a una Partida, útil para verificación y actualización de estado.
    /// </summary>
    /// <param name="idPartida">El ID de la Partida (contexto).</param>
    /// <param name="idMision">El ID de la Misión a buscar.</param>
    /// <returns>La entidad Mision encontrada.</returns>
    /// <exception cref="MisionPartidaExcepcion">Se lanza si no se encuentra la MisiónPartida.</exception>
    Task<Mision> ObtenerMisionPartidaPorId(int idPartida, int idMision);
    
    /// <summary>
    /// Obtiene un listado de todas las misiones diarias asignadas al usuario.
    /// </summary>
    /// <param name="idUsuario">El ID del usuario.</param>
    /// <returns>Una lista de entidades Mision.</returns>
    Task<List<Mision>> ObtenerMisionesDia(int idUsuario);

    /// <summary>
    /// Obtiene un listado de todas las misiones semanales asignadas al usuario.
    /// </summary>
    /// <param name="idUsuario">El ID del usuario.</param>
    /// <returns>Una lista de entidades Mision.</returns>
    Task<List<Mision>> ObtenerMisionesSemana(int idUsuario);

    /// <summary>
    /// Obtiene un listado de todas las misiones mensuales asignadas al usuario.
    /// </summary>
    /// <param name="idUsuario">El ID del usuario.</param>
    /// <returns>Una lista de entidades Mision.</returns>
    Task<List<Mision>> ObtenerMisionesMes(int idUsuario);
    
    /// <summary>
    /// Asigna una lista de misiones maestras a una partida, creando las entidades MisionPartida.
    /// Solo asigna misiones que aún no estén vinculadas a la partida.
    /// </summary>
    /// <param name="misiones">La lista de misiones maestras activas a considerar.</param>
    /// <param name="partida">La entidad Partida a la cual se asignarán las misiones.</param>
    Task AsignarMisiones(List<Mision> misiones, Partida partida);

    /// <summary>
    /// Marca una MisionPartida específica como completada y reclamada, y actualiza el estado en la base de datos.
    /// </summary>
    /// <param name="mision">La entidad Mision de la que se requiere el ID.</param>
    /// <param name="partida">La entidad Partida de la cual se requiere el ID.</param>
    /// <exception cref="MisionPartidaExcepcion">Si no se encuentra la MisionPartida.</exception>
    Task MarcarMisionCompletada(Mision mision, Partida partida);
    
    /// <summary>
    /// Obtiene todas las entidades de unión MisionPartida asociadas a una Partida.
    /// </summary>
    /// <param name="partida">La entidad Partida a buscar.</param>
    /// <returns>Una lista de entidades MisionPartida.</returns>
    Task<List<MisionPartida>> ObtenerMisionesActivasParaPartida(Partida partida);

    /// <summary>
    /// Filtra una lista de misiones candidatas para excluir aquellas que ya fueron asignadas.
    /// </summary>
    /// <param name="reclamables">Lista de misiones candidatas a ser asignadas (o ya asignadas).</param>
    /// <param name="noReclamables">Lista de misiones a excluir (por ejemplo, por no cumplir el requisito).</param>
    /// <returns>Una lista de entidades MisionPartida que pasan el filtro.</returns>
    List<MisionPartida> ProcesarMisionesPartida(List<MisionPartida> reclamables, List<MisionPartida> noReclamables);


}
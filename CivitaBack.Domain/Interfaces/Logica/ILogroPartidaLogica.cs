using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

/// <summary>
/// Define la lógica de negocio para manejar los logros de un jugador dentro de una partida.
/// </summary>
public interface ILogroPartidaLogica
{
    /// <summary>
    /// Obtiene una lista de todos los logros que el jugador AÚN NO ha completado en la partida.
    /// </summary>
    /// <param name="partida">La partida actual, debe incluir Recursos y EstructuraMapa.</param>
    /// <returns>Una lista de DTOs de logros incompletos.</returns>
    Task<List<Logro>> ObtenerLogrosIncompletos(Partida? partida);

    /// <summary>
    /// Obtiene una lista de todos los logros que el jugador YA ha completado y están registrados.
    /// </summary>
    /// <param name="partida">La partida actual, debe incluir Recursos y EstructuraMapa.</param>
    /// <returns>Una lista de DTOs de logros completados.</returns>
    Task<List<Logro>> ObtenerLogrosCompletados(Partida? partida);

    /// <summary>
    /// Obtiene los logros incompletos cuyas condiciones se cumplen AHORA MISMO según el estado de la partida.
    /// </summary>
    /// <param name="partida">La partida actual, debe incluir Recursos y EstructuraMapa.</param>
    /// <returns>Una lista de DTOs de logros que están listos para ser reclamados.</returns>
    Task<List<Logro>> ObtenerLogrosParaReclamar(Partida? partida);

    /// <summary>
    /// Procesa el reclamo de TODOS los logros cuyas condiciones se cumplan.
    /// Aplica las recompensas y guarda los logros como completados.
    /// </summary>
    /// <param name="partida">La partida actual, debe incluir Recursos y EstructuraMapa.</param>
    /// <returns>Una tarea que representa la operación de reclamo.</returns>
    Task ReclamarLogros(Partida? partida);

    /// <summary>
    /// Reinicia (borra) el progreso de todos los logros para una partida específica.
    /// </summary>
    /// <param name="partidaId">El ID de la partida a reiniciar.</param>
    /// <returns>Una tarea que representa la operación de reinicio.</returns>
    Task ReiniciarLogros(int partidaId);

}
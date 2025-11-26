using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface IMisionLogica
{
    /// <summary>
    /// Reinicia el estado de todas las misiones asociadas a partidas.
    /// Resetea los campos 'Reclamado' y 'FechaCompletado' para permitir una nueva asignación.
    /// </summary>
    /// <param name="tipoMision">El tipo de misión a resetear (Diaria, Semanal, Mensual).</param>
    Task ResetMisiones(TipoMision tipoMision);

    /// <summary>
    /// Obtiene un listado completo de todas las misiones maestras del sistema.
    /// </summary>
    /// <returns>Una lista de todas las entidades Mision.</returns>
    Task<List<Mision>> Listado();

    /// <summary>
    /// Crea una nueva misión maestra. Requiere validación de la Condición asociada.
    /// </summary>
    /// <param name="mision">La entidad Mision con los datos para crear.</param>
    /// <exception cref="MisionExcepcion">Se lanza si el título, descripción o Condición son inválidos.</exception>
    Task Crear(Mision mision);

    /// <summary>
    /// Actualiza una misión maestra existente. Requiere validación de la Condición asociada.
    /// </summary>
    /// <param name="mision">La entidad Mision con los nuevos datos.</param>
    /// <param name="idMision">El ID de la Mision a actualizar.</param>
    /// <exception cref="MisionExcepcion">Se lanza si la misión, la Condición o los datos son inválidos.</exception>
    Task Actualizar(Mision mision, int idMision);

    /// <summary>
    /// Obtiene una misión maestra por su ID.
    /// </summary>
    /// <param name="idMision">El ID de la Mision a buscar.</param>
    /// <returns>La entidad Mision.</returns>
    /// <exception cref="MisionExcepcion">Se lanza si no se encuentra la misión.</exception>
    Task<Mision> ObtenerPorId(int idMision);

    /// <summary>
    /// Obtiene un listado de todas las misiones maestras que están marcadas como disponibles.
    /// </summary>
    /// <returns>Una lista de entidades Mision.</returns>
    Task<List<Mision>> ObtenerMisionesDisponibles();
}
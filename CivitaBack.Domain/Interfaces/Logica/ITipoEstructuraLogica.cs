using CivitaBack.Domain.Entidades;
using System; // Para Exception
using System.Collections.Generic; // Para List
using System.Threading.Tasks; // Para Task

namespace CivitaBack.Domain.Interfaces.Logica;

public interface ITipoEstructuraLogica
{
    /// <summary>
    /// Obtiene un Tipo de Estructura específico por su ID.
    /// </summary>
    /// <param name="id">El ID del Tipo de Estructura a buscar.</param>
    /// <returns>La entidad TipoEstructura encontrada.</returns>
    /// <exception cref="Exception">Se lanza si no se encuentra el Tipo de Estructura.</exception>
    Task<TipoEstructura> ObtenerPorId(int id);

    /// <summary>
    /// Guarda un nuevo Tipo de Estructura en la base de datos.
    /// </summary>
    /// <param name="nuevoTipologro">La entidad TipoEstructura con los datos para crear.</param>
    /// <returns>La entidad TipoEstructura recién creada (con su ID).</returns>
    /// <exception cref="Exception">Se lanza si los datos de entrada son inválidos.</exception>
    Task<TipoEstructura> Crear(TipoEstructura nuevoTipologro);

    /// <summary>
    /// Actualiza un Tipo de Estructura existente.
    /// </summary>
    /// <param name="tipoLogro">La entidad con los nuevos datos.</param>
    /// <param name="idTipoLogro">El ID del Tipo de Estructura a actualizar.</param>
    /// <exception cref="Exception">Se lanza si los datos son inválidos o si no se encuentra el Tipo de Estructura.</exception>
    Task Actualizar(TipoEstructura tipoLogro, int idTipoLogro);

    /// <summary>
    /// Obtiene un listado de todos los Tipos de Estructuras.
    /// </summary>
    /// <returns>Una lista de entidades TipoEstructura.</returns>
    Task<List<TipoEstructura>> Listado();

    /// <summary>
    /// Elimina un Tipo de Estructura por su ID.
    /// </summary>
    /// <param name="id">El ID del Tipo de Estructura a eliminar.</param>
    /// <exception cref="Exception">Se lanza si el ID es inválido.</exception>
    Task Eliminar(int id);
}
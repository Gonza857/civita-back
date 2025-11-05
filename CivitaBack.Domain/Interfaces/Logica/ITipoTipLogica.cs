using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones; // Asegúrate de importar tus excepciones
using System.Collections.Generic; // Para List
using System.Threading.Tasks; // Para Task

namespace CivitaBack.Domain.Interfaces.Logica;

public interface ITipoTipLogica
{
    /// <summary>
    /// Obtiene un listado de todos los Tipos de Tips.
    /// </summary>
    /// <returns>Una lista de entidades TipoTip.</returns>
    Task<List<TipoTip>> Listado();

    /// <summary>
    /// Elimina un Tipo de Tip por su ID.
    /// </summary>
    /// <param name="id">El ID del Tipo de Tip a eliminar.</param>
    /// <exception cref="TipoTipException">Se lanza si no se encuentra el Tipo de Tip.</exception>
    Task Eliminar(int id);

    /// <summary>
    /// Guarda un nuevo Tipo de Tip en la base de datos.
    /// </summary>
    /// <param name="tipoTipDto">La entidad TipoTip con los datos para crear.</param>
    Task Guardar(TipoTip tipoTipDto);

    /// <summary>
    /// Obtiene un Tipo de Tip específico por su ID.
    /// </summary>
    /// <param name="id">El ID del Tipo de Tip a buscar.</param>
    /// <returns>La entidad TipoTip encontrada, o null si no existe.</returns>
    Task<TipoTip?> ObtenerPorId(int id);

    /// <summary>
    /// Actualiza un Tipo de Tip existente.
    /// </summary>
    /// <param name="tipoTipDto">La entidad con los nuevos datos.</param>
    /// <param name="id">El ID del Tipo de Tip a actualizar.</param>
    /// <exception cref="TipoTipException">Se lanza si no se encuentra el Tipo de Tip.</exception>
    Task Actualizar(TipoTip tipoTipDto, int id);
}
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones; // Asegúrate de importar tus excepciones
using System; // Para Exception
using System.Collections.Generic; // Para List
using System.Threading.Tasks; // Para Task

namespace CivitaBack.Domain.Interfaces.Logica;

public interface ITipLogica
{
    /// <summary>
    /// Obtiene todos los Tips asociados a un Tipo de Tip específico.
    /// </summary>
    /// <param name="id">El ID del Tipo de Tip (TipoTipId) a buscar.</param>
    /// <returns>Una lista de entidades Tip.</returns>
    Task<List<Tip>> ObtenerMsjPorIdTipo(int id);

    /// <summary>
    /// Obtiene un listado de todos los Tips en el sistema.
    /// </summary>
    /// <returns>Una lista de todas las entidades Tip.</returns>
    Task<List<Tip>> Listado();

    /// <summary>
    /// Guarda un nuevo Tip en la base de datos.
    /// </summary>
    /// <param name="tip">La entidad Tip con los datos para crear.</param>
    /// <exception cref="Exception">Se lanza si el TipoTip asociado no se encuentra.</exception>
    /// <exception cref="ErrorInternoException">Se lanza si ocurre un error en la base de datos.</exception>
    Task Crear(Tip tip);

    /// <summary>
    /// Actualiza un Tip existente.
    /// </summary>
    /// <param name="tip">La entidad Tip con los nuevos datos.</param>
    /// <param name="id">El ID del Tip a actualizar.</param>
    /// <exception cref="Exception">Se lanza si el TipoTip asociado o el Tip a actualizar no se encuentran.</exception>
    /// <exception cref="ErrorInternoException">Se lanza si ocurre un error en la base de datos.</exception>
    Task Actualizar(Tip tip, int id);

    /// <summary>
    /// Obtiene un Tip específico por su ID.
    /// </summary>
    /// <param name="id">El ID del Tip a buscar.</param>
    /// <returns>La entidad Tip encontrada, o null si no existe.</returns>
    Task<Tip?> ObtenerPorIdTipo(int id);
    
    Task Eliminar(int id);
}
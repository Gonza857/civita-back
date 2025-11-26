using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface ITipoLogroLogica
{
/// <summary>
    /// Obtiene un Tipo de Logro específico por su ID.
    /// </summary>
    /// <param name="id">El ID del Tipo de Logro a buscar.</param>
    /// <returns>La entidad TipoLogro.</returns>
    /// <exception cref="TipoLogroException">Se lanza si no se encuentra el Tipo de Logro.</exception>
    Task<TipoLogro> ObtenerPorId(int id);
    
    /// <summary>
    /// Guarda un nuevo Tipo de Logro en la base de datos.
    /// </summary>
    /// <param name="nuevoTipologro">La entidad TipoLogro con los datos para crear.</param>
    /// <returns>La entidad TipoLogro recién creada (con su ID).</returns>
    /// <exception cref="TipoLogroException">Se lanza si los datos son inválidos.</exception>
    /// <exception cref="AccesoDenegadoExcepcion">Se lanza si el usuario no tiene privilegios de administrador.</exception>
    Task<TipoLogro> Guardar(TipoLogro nuevoTipologro);

    /// <summary>
    /// Actualiza un Tipo de Logro existente.
    /// </summary>
    /// <param name="tipoLogro">La entidad con los nuevos datos.</param>
    /// <param name="idTipoLogro">El ID del Tipo de Logro a actualizar.</param>
    /// <exception cref="TipoLogroException">Se lanza si los datos o el ID son inválidos, o si no se encuentra el Tipo de Logro.</exception>
    /// <exception cref="AccesoDenegadoExcepcion">Se lanza si el usuario no tiene privilegios de administrador.</exception>
    Task Actualizar(TipoLogro tipoLogro, int idTipoLogro);

    /// <summary>
    /// Obtiene un listado de todos los Tipos de Logros.
    /// </summary>
    /// <returns>Una lista de entidades TipoLogro.</returns>
    Task<List<TipoLogro>> ObtenerTiposLogro();

    /// <summary>
    /// Elimina un Tipo de Logro por su ID.
    /// </summary>
    /// <param name="id">El ID del Tipo de Logro a eliminar.</param>
    /// <exception cref="TipoLogroException">Se lanza si el ID es inválido.</exception>
    /// <exception cref="AccesoDenegadoExcepcion">Se lanza si el usuario no tiene privilegios de administrador.</exception>
    Task Eliminar(int id);
}
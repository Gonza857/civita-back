namespace CivitaBack.Utils;

/// <summary>
/// Define las operaciones de escritura genéricas para un repositorio.
/// Estas operaciones solo preparan el DbContext y no guardan cambios.
/// </summary>
/// <typeparam name="TDominio">La entidad de Dominio (pura)</typeparam>
public interface IRepositorioBase<TDominio> where TDominio : class
{
    /// <summary>
    /// Prepara una nueva entidad para ser agregada (sin guardar).
    /// </summary>
    Task Agregar(TDominio entidad);

    /// <summary>
    /// Prepara una colección de nuevas entidades para ser agregadas (sin guardar).
    /// </summary>
    Task AgregarVarios(ICollection<TDominio> entidades);

    /// <summary>
    /// Prepara una entidad existente para ser actualizada (sin guardar).
    /// </summary>
    Task Actualizar(TDominio entidad);
    
    /// <summary>
    /// Prepara una colección de entidades existentes para ser eliminadas (sin guardar).
    /// </summary>
    /// <remarks>
    /// Este método reemplaza a 'Eliminar(int id)', ya que requiere
    /// las instancias de las entidades de dominio a borrar.
    /// </remarks>
    Task EliminarVarios(ICollection<TDominio> entidades);
    
    Task ActualizarVarios(ICollection<TDominio> entidades);
}
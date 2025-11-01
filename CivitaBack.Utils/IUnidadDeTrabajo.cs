namespace CivitaBack.Utils;

public interface IUnidadDeTrabajo
{
    /// <summary>
    /// Guarda todos los cambios preparados (Agregados, Actualizados, Eliminados)
    /// en la base de datos dentro de una única transacción.
    /// </summary>
    /// <returns>El número de filas afectadas.</returns>
    Task<int> CommitAsync();
    
    // También puedes llamarlo 'GuardarGeneralAsync()' si te resulta más claro.
}
namespace CivitaBack.Data.Repositorio;

public interface IRepositorioBase<T> where T : class
{
    Task<T?> ObtenerPorId(int id);
    Task<List<T>> ObtenerTodos();
    Task Actualizar(T entidad);
    Task Eliminar(int id);
    Task Agregar(T entidad);
    Task AgregarVarios(List<T> entidades);
    Task Guardar();
}

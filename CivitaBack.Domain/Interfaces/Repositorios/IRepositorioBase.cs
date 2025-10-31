namespace CivitaBack.Domain.Interfaces.Repositorios;

public interface IRepositorioBase<T> where T : class
{
    Task<T?> ObtenerPorId(int id);
    Task<List<T>> ObtenerTodos();
    Task Agregar(T entidad);
    Task AgregarVarios(ICollection<T> entidades);
    Task Actualizar(T entidad);
    Task Eliminar(int id);
}

namespace CivitaBack.Data.Repositorio;

public interface IRepositorioBase<TDominio> where TDominio : class
{
    Task<TDominio?> ObtenerPorId(int id);
    Task<List<TDominio>> ObtenerTodos();
    Task Agregar(TDominio entidad);
    Task AgregarVarios(ICollection<TDominio> entidades);
    Task Actualizar(TDominio entidad);
    Task Eliminar(int id);
}

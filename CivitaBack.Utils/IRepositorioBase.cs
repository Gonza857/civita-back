using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.Repositorio;

public interface IRepositorioBase<T> where T : class
{
    Task<T> ObtenerPorId(int id);
    Task<List<T>> ObtenerTodos();
    Task Actualizar(T entidad);
    Task Eliminar(int id);
    Task Guardar(T entidad);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.Repositorio;

public interface IRepositorioBase<T> where T : class
{
    T ObtenerPorId(int id);
    List<T> ObtenerTodos();
    void Actualizar(T entidad);
    void Eliminar(int id);
    void Guardar(T entidad);
}

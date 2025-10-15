using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;

namespace CivitaBack.Data.Repositorio;

public interface IEstructuraRepositorio : IRepositorioBase<Estructura>
{

}
public class EstructuraRepositorio : IEstructuraRepositorio
{
    private readonly AppDbContext _context;

    public EstructuraRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public void Actualizar(Estructura entidad)
    {
        throw new NotImplementedException();
    }

    public void Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public void Guardar(Estructura entidad)
    {
        throw new NotImplementedException();
    }

    public Estructura ObtenerPorId(int id)
    {
        return _context.Estructura.FirstOrDefault(e => e.Id == id);
    }

    public List<Estructura> ObtenerTodos()
    {
        throw new NotImplementedException();
    }
}

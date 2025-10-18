using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

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

    public async Task Actualizar(Estructura entidad)
    {
        throw new NotImplementedException();
    }

    public async Task Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public async Task Guardar(Estructura entidad)
    {
        throw new NotImplementedException();
    }

    public async Task<Estructura> ObtenerPorId(int id)
    {
        return await _context.Estructura.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Estructura>> ObtenerTodos()
    {
        throw new NotImplementedException();
    }
}

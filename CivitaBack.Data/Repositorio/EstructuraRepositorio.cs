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
public class EstructuraRepositorio : GenericoRepositorio, IEstructuraRepositorio
{
    public EstructuraRepositorio(AppDbContext context) : base(context) { }
    
    public async Task Actualizar(Estructura entidad)
    {
        entidad.Editado = DateTime.UtcNow;
        await _context.Estructura.AddAsync(entidad);
        await base.GuardarCambiosAsync();
    }

    public async Task Eliminar(int id)
    {
        var estructura = await _context.Estructura
            .FirstOrDefaultAsync(tl => tl.Id == id);
        
        if (estructura != null)
        { 
            _context.Estructura.Remove(estructura);
            await base.GuardarCambiosAsync();
        }
    }

    public async Task Agregar(Estructura entidad)
    {
        await _context.Estructura.AddAsync(entidad);
        await _context.SaveChangesAsync();
    }

    public Task AgregarVarios(List<Estructura> entidades)
    {
        throw new NotImplementedException();
    }

    public Task Guardar()
    {
        throw new NotImplementedException();
    }

    public async Task<Estructura?> ObtenerPorId(int id)
    {
        return await _context.Estructura
            .Include(e => e.TipoEstructura)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Estructura>> ObtenerTodos()
    {
        return await _context.Estructura
            .Include(e => e.TipoEstructura)
            .ToListAsync();
    }
}

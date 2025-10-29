using CivitaBack.Domain.Entities;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio;

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

    public async Task Guardar(Estructura entidad)
    {
        await _context.Estructura.AddAsync(entidad);
        await _context.SaveChangesAsync();
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

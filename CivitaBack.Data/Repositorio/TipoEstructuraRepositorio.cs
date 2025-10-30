using CivitaBack.Domain.Entities;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio;

public class TipoEstructuraRepositorio : GenericoRepositorio, ITipoEstructuraRepositorio
{
    public TipoEstructuraRepositorio(AppDbContext context) : base(context) { }

    public async Task<TipoEstructura?> ObtenerPorId(int id)
    {
        return await _context.TipoEstructura
                .FirstOrDefaultAsync(tl => tl.Id == id);
    }

    public async Task<List<TipoEstructura>> ObtenerTodos()
    {
        return await _context.TipoEstructura
            .ToListAsync();
    }

    public async Task Actualizar(TipoEstructura entidad)
    {
        entidad.Editado = DateTime.UtcNow;
        _context.TipoEstructura.Update(entidad);
        await base.GuardarCambiosAsync();
    }

    public async Task Eliminar(int id)
    {
        var tipoEstructura = await _context.TipoEstructura
            .FirstOrDefaultAsync(tl => tl.Id == id);
        
        if (tipoEstructura != null)
        { 
            _context.TipoEstructura.Remove(tipoEstructura);
            await base.GuardarCambiosAsync();
        }
    }

    public async Task Agregar(TipoEstructura entidad)
    {
        await _context.TipoEstructura.AddAsync(entidad);
        await base.GuardarCambiosAsync();
    }

    public Task AgregarVarios(List<TipoEstructura> entidades)
    {
        throw new NotImplementedException();
    }

    public Task Guardar()
    {
        throw new NotImplementedException();
    }
}
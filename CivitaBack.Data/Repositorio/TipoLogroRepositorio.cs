using CivitaBack.Domain.Entities;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio;

public class TipoLogroRepositorio : GenericoRepositorio, ITipoLogroRepositorio
{
    public TipoLogroRepositorio(AppDbContext context) : base(context) { }
    
    public async Task Actualizar(TipoLogro entidad)
    {
        entidad.Editado = DateTime.UtcNow;
        _context.TipoLogro.Update(entidad);
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(int id)
    {
        var tipoLogro = await _context.TipoLogro
            .FirstOrDefaultAsync(tl => tl.Id == id);
        
        if (tipoLogro != null)
        { 
            _context.TipoLogro.Remove(tipoLogro);
            await _context.SaveChangesAsync();
        }
    }

    public Task Guardar(TipoLogro entidad)
    {
        throw new NotImplementedException();
    }

    public async Task Agregar(TipoLogro tipoLogro)
    {
        await _context.TipoLogro.AddAsync(tipoLogro);
        await _context.SaveChangesAsync();
    }

    public Task AgregarVarios(List<TipoLogro> entidades)
    {
        throw new NotImplementedException();
    }

    public Task Guardar()
    {
        throw new NotImplementedException();
    }

    public async Task<TipoLogro?> ObtenerPorId(int id)
    {
        return await _context.TipoLogro
            .FirstOrDefaultAsync(tl => tl.Id == id);
    }
    
    public async Task<List<TipoLogro>> ObtenerTodos()
    {
        return await _context.TipoLogro.ToListAsync();
    }
}

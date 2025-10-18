using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface ITipoLogroRepositorio : IRepositorioBase<TipoLogro>
{

}

public class TipoLogroRepositorio : ITipoLogroRepositorio
{
    private readonly AppDbContext _context;

    public TipoLogroRepositorio(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task Actualizar(TipoLogro entidad)
    {
        await _context.TipoLogro.AddAsync(entidad);
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
    
    public async Task Guardar(TipoLogro tipoLogro)
    {
        await _context.TipoLogro.AddAsync(tipoLogro);
        await _context.SaveChangesAsync();
    }

    public async Task<TipoLogro?> ObtenerPorId(int Id)
    {
        return await _context.TipoLogro
            .FirstOrDefaultAsync(tl => tl.Id == Id);
    }
    
    public async Task<List<TipoLogro>> ObtenerTodos()
    {
        return await _context.TipoLogro.ToListAsync();
    }
}

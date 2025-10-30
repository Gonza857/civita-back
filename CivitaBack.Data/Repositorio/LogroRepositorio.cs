using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface ILogroRepositorio : IRepositorioBase<Logro>
{
    Task<bool> ExisteLogroEnCumplidos(int idLogro);
}
public class LogroRepositorio : ILogroRepositorio
{
    private readonly AppDbContext _context;

    public LogroRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task Actualizar(Logro logro)
    {
        logro.Editado = DateTime.UtcNow;
        _context.Logro.Update(logro);
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(int id)
    {
        var logro = await _context.Logro.FirstOrDefaultAsync(tl => tl.Id == id);
        if (logro != null)
        {
            _context.Logro.Remove(logro);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Agregar(Logro entidad)
    {
        await _context.Logro.AddAsync(entidad);
        await _context.SaveChangesAsync();
    }

    public Task AgregarVarios(List<Logro> entidades)
    {
        throw new NotImplementedException();
    }

    public Task Guardar()
    {
        throw new NotImplementedException();
    }

    public async Task<Logro> ObtenerPorId(int id)
    {
        return await _context.Logro
            .Include(tl => tl.TipoLogro)
            .Include(tl => tl.Condicion)
            .FirstOrDefaultAsync(tl => tl.Id == id);
    }

    public async Task<List<Logro>> ObtenerTodos()
    {
        return await _context.Logro
           .Include(l => l.Condicion)
               .ThenInclude(c => c.Recompensa)       // Recompensa de la Condicion
           .Include(l => l.Condicion)
               .ThenInclude(c => c.Estructura)       // Estructura de la Condicion
           .Include(l => l.TipoLogro)
           .ToListAsync();
    }

    public async Task<bool> ExisteLogroEnCumplidos(int idLogro)
    {
        return await _context.LogroPartida.AnyAsync(lp => lp.LogroId == idLogro);
    }
}

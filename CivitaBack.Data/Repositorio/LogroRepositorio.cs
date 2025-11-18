using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio;

public class LogroRepositorio 
    : GenericoRepositorio<Logro, LogroEF>, ILogroRepositorio
{
    public LogroRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public async Task AgregarVarios(List<Logro> entidades)
    {
        await base.AgregarVarios(entidades);
    }
    
    public async Task<Logro?> ObtenerPorId(int id)
    {
        LogroEF? logro = await _context.Logro
            .Include(tl => tl.TipoLogro)
            .Include(tl => tl.Condicion)
            .FirstOrDefaultAsync(tl => tl.Id == id);
        if (logro == null) return null;
        return base.Mapear<Logro>(logro);
    }

    public override async Task<List<Logro>> ObtenerTodos()
    {
        List<LogroEF> logros = await _context.Logro
           .Include(l => l.Condicion)
               .ThenInclude(c => c.Recompensas)       // Recompensa de la Condicion
           .Include(l => l.Condicion)
               .ThenInclude(c => c.Estructura)       // Estructura de la Condicion
           .Include(l => l.TipoLogro)
           .ToListAsync();
        
        return base.MapearLista<Logro>(logros);
    }

    public async Task<bool> ExisteLogroEnCumplidos(int idLogro)
    {
        return await _context.LogroPartida.AnyAsync(lp => lp.LogroId == idLogro);
    }
}

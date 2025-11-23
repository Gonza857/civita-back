using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class TipRepositorio
    : GenericoRepositorio<Tip, TipEF>, ITipsRepositorio
{
    public TipRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public async Task<List<Tip>> ObtenerMsjPorIdTipo(int idTipo, bool ascendente = true)
    {
        IQueryable<TipEF> consulta = _context.Tip
            .Include(x => x.TipoTip)
            .Where(x => x.TipoId == idTipo);
        
        if (ascendente)
            consulta = consulta.OrderBy(x => x.Orden); 
        else
            consulta = consulta.OrderByDescending(x => x.Orden);
        
        List<TipEF> tipEf = await consulta
            .AsNoTracking()
            .ToListAsync();
        
        return base.MapearLista<Tip>(tipEf);
    }

    public async Task<Tip?> ObtenerPorId(int id)
    {
        var tipEf = await _context.Tip
            .Where(t => t.Id == id)
            .Include(t => t.TipoTip)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return base.Mapear<Tip>(tipEf);
    }

    public override async Task<List<Tip>> ObtenerTodos()
    {
        var tipsEf = await _context.Tip
            .Include(t => t.TipoTip)
            .ToListAsync();
        return base.MapearLista<Tip>(tipsEf);
    }
    
}
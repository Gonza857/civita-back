using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class TipsRepositorio
    : GenericoRepositorio<Tip, TipEF>, ITipsRepositorio
{
    public TipsRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public async Task<List<Tip>> ObtenerMsjPorIdTipo(int idTipo)
    {
        var tipEf =  await _context.Tip
            .Include(x => x.TipoTip)
            .Where(x => x.TipoId == idTipo)
            .ToListAsync();
        return base.MapearLista<Tip>(tipEf);
    }

    public override async Task<Tip?> ObtenerPorId(int id)
    {
        var tipEf = await _context.Tip
            .Where(t => t.Id == id)
            .Include(t => t.TipoTip)
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
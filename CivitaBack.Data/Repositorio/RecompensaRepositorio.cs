using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class RecompensaRepositorio : GenericoRepositorio<Recompensa, RecompensaEF>, IRecompensaRepositorio
{
    public RecompensaRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public async Task<Recompensa?> ObtenerPorId(int id)
    {
        var r = await base.ObtenerPorId((r) => r.Id == id);
        return base.Mapear<Recompensa>(r);
    }
    
    public override async Task<List<Recompensa>> ObtenerTodos()
    {
        var listaEf = await _dbSet
            .Include(c => c.Estructura)
            .AsNoTracking()
            .ToListAsync();

        return MapearLista<Recompensa>(listaEf);
    }

    public async Task<List<Recompensa>> ObtenerVariosPorIds(List<int> ids)
    {
        var listaEf = await _dbSet
            .Where(c => ids.Contains(c.Id))
            .AsNoTracking()
            .ToListAsync();
        return base.MapearLista<Recompensa>(listaEf);
    }
}
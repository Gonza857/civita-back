using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class EstructuraRepositorio : GenericoRepositorio<Estructura, EstructuraEF>, IEstructuraRepositorio
{
    public EstructuraRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }
    
    public async Task<Estructura?> ObtenerPorId(int id)
    {
        var entidadEF = await _dbSet
            .Include(e => e.TipoEstructura)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        return Mapear<Estructura>(entidadEF);
    }

    public override async Task<List<Estructura>> ObtenerTodos()
    {
        var estructurasEF = await _dbSet
            .Include(e => e.TipoEstructura)
            .AsNoTracking()
            .ToListAsync();

        return MapearLista<Estructura>(estructurasEF);
    }
}

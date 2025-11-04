using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio;

public class TipoEstructuraRepositorio
    : GenericoRepositorio<TipoEstructura, TipoEstructuraEF>, ITipoEstructuraRepositorio
{
    public TipoEstructuraRepositorio (AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public async Task<TipoEstructura?> ObtenerPorId(int id)
    {
        var estructura = await _context.TipoEstructura
                .FirstOrDefaultAsync(tl => tl.Id == id);
        return base.Mapear<TipoEstructura>(estructura);
    }
    
}
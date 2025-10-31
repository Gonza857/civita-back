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

    public override async Task<TipoEstructura?> ObtenerPorId(int id)
    {
        var estructura = await _context.TipoEstructura
                .FirstOrDefaultAsync(tl => tl.Id == id);
        return base.Mapear<TipoEstructura>(estructura);
    }

    public override async Task<List<TipoEstructura>> ObtenerTodos()
    {
        return base.MapearLista<TipoEstructura>(await base.ObtenerTodos());
    }

    public async Task Actualizar(TipoEstructura entidad)
    {
        await base.Actualizar(entidad);
    }

    public async Task Eliminar(int id)
    {
        await base.Eliminar(id);
    }
    
    public async Task Agregar(TipoEstructura entidad)
    {
        await base.Agregar(entidad);
    }

    public async Task AgregarVarios(List<TipoEstructura> entidades)
    {
        await base.AgregarVarios(entidades);
    }
    
}
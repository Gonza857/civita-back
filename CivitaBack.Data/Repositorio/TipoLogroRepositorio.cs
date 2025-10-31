using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class TipoLogroRepositorio 
    : GenericoRepositorio<TipoLogro, TipoLogroEF>, ITipoLogroRepositorio 
{
    public TipoLogroRepositorio (AppDbContext context, IMapper mapper) : base(context, mapper) { }
    
    public async Task Actualizar(TipoLogro entidad)
    {
        await base.Actualizar(entidad);
    }

    public async Task Eliminar(int id)
    {
        await base.Eliminar(id);
    }

    public Task Guardar(TipoLogro entidad)
    {
        throw new NotImplementedException();
    }

    public async Task Agregar(TipoLogro tipoLogro)
    {
        await base.Agregar(tipoLogro);
    }

    public Task AgregarVarios(List<TipoLogro> entidades)
    {
        throw new NotImplementedException();
    }

    public Task Guardar()
    {
        throw new NotImplementedException();
    }

    public override async Task<TipoLogro?> ObtenerPorId(int id)
    {
        var tipoLogroEf = await _context.TipoLogro
            .FirstOrDefaultAsync(tl => tl.Id == id);
        return base.Mapear<TipoLogro>(tipoLogroEf);
    }
    
}

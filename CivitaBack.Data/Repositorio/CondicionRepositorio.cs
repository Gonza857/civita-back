using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entities;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class CondicionRepositorio : GenericoRepositorio<Condicion, CondicionEF>, ICondicionRepositorio
{
    public CondicionRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public override async Task<Condicion?> ObtenerPorId(int id)
    {
        var entidadEF = await _dbSet
            .Include(c => c.Estructura) 
            .AsNoTracking()
            .FirstOrDefaultAsync(tl => tl.Id == id); 

        return Mapear<Condicion>(entidadEF);
    }

    public override async Task<List<Condicion>> ObtenerTodos()
    {
        var listaEF = await _dbSet
            .Where(c => !c.EsRecompensa)
            .Include(c => c.Estructura)
            .Include(c => c.Recompensa)
            .AsNoTracking()
            .ToListAsync();

        return MapearLista<Condicion>(listaEF);
    }

    public new Task Actualizar(Condicion entidad)
    {
        return base.Actualizar(entidad);
    }

    public new Task Eliminar(int id)
    {
        return base.Eliminar(id);
    }

    public Task Guardar(Condicion entidad)
    {
        throw new NotImplementedException();
    }

    public new Task Agregar(Condicion entidad)
    {
        return base.Agregar(entidad);
    }

    public Task AgregarVarios(List<Condicion> entidades)
    {
        throw new NotImplementedException();
    }

    public Task Guardar()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Condicion>> ObtenerTodasRecompensas()
    {
        var listaEF = await _dbSet
            .Where(c => c.EsRecompensa)
            .AsNoTracking()
            .ToListAsync();

        return MapearLista<Condicion>(listaEF);
    }
}
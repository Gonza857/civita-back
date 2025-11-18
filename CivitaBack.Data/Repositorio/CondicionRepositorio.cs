using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class CondicionRepositorio : GenericoRepositorio<Condicion, CondicionEF>, ICondicionRepositorio
{
    public CondicionRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public async Task<Condicion?> ObtenerPorId(int id)
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
            .Include(c => c.Estructura)
            .Include(c => c.Recompensas)
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
    

    public override async Task Agregar(Condicion entidad)
    {
        var condicionEf = _mapper.Map<CondicionEF>(entidad);
        if (condicionEf is AuditableEF auditable)
        {
            auditable.Creado = DateTime.UtcNow;
            auditable.Editado = DateTime.UtcNow; 
        }
        var recompensasEf = condicionEf.Recompensas.ToList();
        condicionEf.Recompensas.Clear();
        await _dbSet.AddAsync(condicionEf);
        foreach (var recompensaEF in recompensasEf)
        {
            _context.Entry(recompensaEF).State = EntityState.Unchanged; 
            condicionEf.Recompensas.Add(recompensaEF);
        }
        
    }
    
}
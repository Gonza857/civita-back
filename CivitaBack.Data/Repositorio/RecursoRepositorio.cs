using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio;

public class RecursoRepositorio 
    : GenericoRepositorio<Recurso, RecursoEF>, IRecursoRepositorio
{
    public RecursoRepositorio (AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public async Task<Recurso?> ObtenerPorId(int id)
    {
        RecursoEF? recursoEf = await _context.Recurso
            .Include(r => r.Partida)
            .Where(r => r.PartidaId == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return base.Mapear<Recurso>(recursoEf);
    }

    public Task Guardar(Recurso entidad)
    {
        throw new NotImplementedException();
    }

    public async Task<Recurso> ObtenerRecursosPartida(int idPartida)
    {
        var recursoEf = await _context.Recurso
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.PartidaId == idPartida);
        return base.Mapear<Recurso>(recursoEf);
    }
    
}
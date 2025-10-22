using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;
public interface IRecursoRepositorio : IRepositorioBase<Recurso>
{
    Task GuardarRecurso(Recurso recurso);
    Task<Recurso> ObtenerRecursosPartida(int idPartida);
}
public class RecursoRepositorio : IRecursoRepositorio
{
    private readonly AppDbContext _context;

    public RecursoRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task Actualizar(Recurso entidad)
    {
        entidad.Editado = DateTime.UtcNow;
        _context.Recurso.Update(entidad);
        await _context.SaveChangesAsync();
    }
    public Task Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public Task Guardar(Recurso entidad)
    {
        throw new NotImplementedException();
    }

    public async Task GuardarRecurso(Recurso recurso)
    {
        await _context.Recurso.AddAsync(recurso);
        await _context.SaveChangesAsync();
    }

    public async Task<Recurso?> ObtenerPorId(int id)
    {
        return await _context.Recurso
            .Where(r => r.PartidaId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Recurso> ObtenerRecursosPartida(int idPartida)
    {
        return await _context.Recurso
            .FirstOrDefaultAsync(r => r.PartidaId == idPartida);
    }

    public Task<List<Recurso>> ObtenerTodos()
    {
        throw new NotImplementedException();
    }
}
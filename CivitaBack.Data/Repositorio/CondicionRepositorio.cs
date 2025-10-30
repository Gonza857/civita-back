using CivitaBack.Domain.Entities;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class CondicionRepositorio : GenericoRepositorio, ICondicionRepositorio
{
    public CondicionRepositorio(AppDbContext context) : base(context) { }

    public async Task<Condicion?> ObtenerPorId(int id)
    {
        return await _context.Condicion
            .Include(c => c.Estructura)
            .FirstOrDefaultAsync(tl => tl.Id == id);
    }

    public async Task<List<Condicion>> ObtenerTodos()
    {
        return await _context.Condicion
            .Where(c => !c.EsRecompensa)
            .Include(c => c.Estructura)
            .Include(c => c.Recompensa)
            .ToListAsync();
    }

    public async Task Actualizar(Condicion entidad)
    {
        entidad.Editado = DateTime.UtcNow;
        _context.Condicion.Update(entidad);
        await base.GuardarCambiosAsync();
    }

    public async Task Eliminar(int id)
    {
        var entidad = await _context.Condicion.FirstOrDefaultAsync(tl => tl.Id == id);
        if (entidad != null)
        {
            _context.Condicion.Remove(entidad);
            await base.GuardarCambiosAsync();
        }
    }

    public Task Guardar(Condicion entidad)
    {
        throw new NotImplementedException();
    }

    public async Task Agregar(Condicion entidad)
    {
        entidad.Creado = DateTime.UtcNow;
        await _context.Condicion.AddAsync(entidad);
        await base.GuardarCambiosAsync();
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
        return await _context.Condicion
            .Where(c => c.EsRecompensa)
            .ToListAsync();
    }
}
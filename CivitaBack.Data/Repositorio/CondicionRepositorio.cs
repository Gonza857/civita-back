using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface ICondicionRepositorio : IRepositorioBase<Condicion>
{

}
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
            .Include(c => c.Estructura)
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

    public async Task Guardar(Condicion entidad)
    {
        entidad.Creado = DateTime.UtcNow;
        await _context.Condicion.AddAsync(entidad);
        await base.GuardarCambiosAsync();
    }
}
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface ITipoTipRepositorio : IRepositorioBase<TipoTip>
{

}

public class TipoTipRepositorio : GenericoRepositorio, ITipoTipRepositorio
{
    public TipoTipRepositorio(AppDbContext context) : base(context) { }


    public async Task<TipoTip?> ObtenerPorId(int id)
    {
        return await _context.TipoTip
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<TipoTip>> ObtenerTodos()
    {
        return await _context.TipoTip
            .ToListAsync();
    }

    public async Task Actualizar(TipoTip entidad)
    {
        entidad.Editado = DateTime.UtcNow;
        _context.TipoTip.Update(entidad);   
        await base.GuardarCambiosAsync();
    }

    public async Task Eliminar(int id)
    {
        throw new NotImplementedException();
        // _context.TipoTip.Remove(id);
        // await base.GuardarCambiosAsync();
    }

    public async Task Agregar(TipoTip entidad)
    {
        await _context.TipoTip.AddAsync(entidad);
        await base.GuardarCambiosAsync();
    }

    public Task AgregarVarios(List<TipoTip> entidades)
    {
        throw new NotImplementedException();
    }

    public Task Guardar()
    {
        throw new NotImplementedException();
    }
}
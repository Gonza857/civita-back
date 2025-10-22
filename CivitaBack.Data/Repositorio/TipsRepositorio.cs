using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio
{
    public interface ITipsRepositorio : IRepositorioBase<Tip>
    {
        Task<List<Tip>> ObtenerMsjPorIdTipo(int idTipo);
    }
    public class TipsRepositorio : GenericoRepositorio, ITipsRepositorio
    {
        public TipsRepositorio(AppDbContext context) : base(context) { }
        
        public async Task<List<Tip>> ObtenerMsjPorIdTipo(int idTipo)
        {
            return await _context.Tip
                .Include(x => x.TipoTip)
                .Where(x => x.TipoId == idTipo)
                .ToListAsync();
        }
        
        public async Task<Tip?> ObtenerPorId(int id)
        {
            return await _context.Tip
                .Where(t => t.Id == id)
                .Include(t => t.TipoTip)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Tip>> ObtenerTodos()
        {
            return await _context.Tip
                .Include(t => t.TipoTip)
                .ToListAsync();
        }

        public async Task Actualizar(Tip entidad)
        {
            entidad.Editado = DateTime.UtcNow;
            _context.Tip.Update(entidad);
            await base.GuardarCambiosAsync();
        }

        public async Task Eliminar(int id)
        {
            var t = await _context.TipoTip.FindAsync(id);
            if (t != null)
            {
                _context.TipoTip.Remove(t);
                await base.GuardarCambiosAsync();
            }
        }

        public async Task Guardar(Tip entidad)
        {
            await _context.Tip.AddAsync(entidad);
            await base.GuardarCambiosAsync();
        }
    }
}

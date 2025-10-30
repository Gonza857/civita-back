using CivitaBack.Domain.Entities;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio
{
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

        public Task Guardar(Tip entidad)
        {
            throw new NotImplementedException();
        }

        public async Task Agregar(Tip entidad)
        {
            await _context.Tip.AddAsync(entidad);
            await base.GuardarCambiosAsync();
        }

        public Task AgregarVarios(List<Tip> entidades)
        {
            throw new NotImplementedException();
        }

        public Task Guardar()
        {
            throw new NotImplementedException();
        }
    }
}

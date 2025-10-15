using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio
{
    public interface ICicloRepositorio
    {
        Task GuardarCambiosAsync();
        Task<List<Partida>> ObtenerPartidasConEstructuras();
    }
    public class CicloRepositorio : ICicloRepositorio
    {
        private readonly AppDbContext _context;
        public CicloRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Partida>> ObtenerPartidasConEstructuras()
        {
            return await _context.Partida.Include(p => p.Recursos)
                .Include(p => p.EstructuraMapa)
                .ThenInclude(em => em.Estructura)
                .ThenInclude(e => e.TipoEstructura)
                .ToListAsync();
        }
    }
}

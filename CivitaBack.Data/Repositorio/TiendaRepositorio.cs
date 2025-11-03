using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio
{
    public class TiendaRepositorio : GenericoRepositorio<Tienda, TiendaEF>, ITiendaRepositorio
    {
        public TiendaRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<Tienda?> ObtenerPorId(int id)
        {
            var tienda = await _dbSet
                .AsNoTracking()
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            return Mapear<Tienda?>(tienda);
        }

    }
}

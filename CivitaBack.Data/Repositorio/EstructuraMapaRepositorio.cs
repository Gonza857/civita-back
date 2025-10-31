using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Common;
using CivitaBack.Domain.Entities;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class EstructuraMapaRepositorio : GenericoRepositorio<EstructuraMapa, EstructuraMapaEF>, IEstructuraMapaRepositorio
{
    public EstructuraMapaRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public void AgregarUnica(EstructuraMapa em)
    {
        if (em is Auditable auditable) auditable.Editado = DateTime.UtcNow;

        var emEF = Mapear<EstructuraMapaEF>(em);

        _dbSet.Update(emEF);
    }

    public void RemoverEliminadas(List<EstructuraMapa> emList)
    {
        var eliminadasEF = Mapear<List<EstructuraMapaEF>>(emList);

        _context.EstructuraMapa.RemoveRange(eliminadasEF);
    }

    public async Task AgregarNuevas(List<EstructuraMapa> emList)
    {
        await base.AgregarVarios(emList);
    }

    public async Task EliminarPorPartidaIdAsync(int partidaId)
    {
        var existentesEF = await _dbSet
            .Where(e => e.PartidaId == partidaId)
            .ToListAsync();

        _dbSet.RemoveRange(existentesEF);
    }

    public async Task AgregarVariasAsync(List<EstructuraMapa> estructuras)
    {
        await base.AgregarVarios(estructuras);
    }

    public async Task<EstructuraMapa?> ObtenerCoincidenteAsync(int partidaId, int estructuraId, int x, int y, int width, int height)
    {
        var entidadEF = await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.PartidaId == partidaId &&
                e.EstructuraId == estructuraId &&
                e.X == x &&
                e.Y == y &&
                e.Width == width &&
                e.Height == height
            );

        return Mapear<EstructuraMapa>(entidadEF);
    }

    public async Task EliminarAsync(EstructuraMapa entidad)
    {
        var entidadEF = Mapear<EstructuraMapaEF>(entidad);

        _dbSet.Remove(entidadEF);
    }

}

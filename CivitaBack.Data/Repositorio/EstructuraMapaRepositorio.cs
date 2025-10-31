using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
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
        _context.EstructuraMapa.RemoveRange(emList);
    }

    public void AgregarNuevas(List<EstructuraMapa> emList)
    {
        _context.EstructuraMapa.AddRange(emList);
    }

    public async Task EliminarPorPartidaIdAsync(int partidaId)
    {
        var existentes = _context.EstructuraMapa.Where(e => e.PartidaId == partidaId);
        _context.EstructuraMapa.RemoveRange(existentes);
        await _context.SaveChangesAsync();
    }

    public async Task AgregarVariasAsync(List<EstructuraMapa> estructuras)
    {
        await _context.EstructuraMapa.AddRangeAsync(estructuras);
        await _context.SaveChangesAsync();
    }

    public async Task<EstructuraMapa?> ObtenerCoincidenteAsync(int partidaId, int estructuraId, int x, int y, int width, int height)
    {
        return await _context.EstructuraMapa.FirstOrDefaultAsync(e =>
            e.PartidaId == partidaId &&
            e.EstructuraId == estructuraId &&
            e.X == x &&
            e.Y == y &&
            e.Width == width &&
            e.Height == height
        );
    }

    public async Task EliminarAsync(EstructuraMapa entidad)
    {
        _context.EstructuraMapa.Remove(entidad);
        await Task.CompletedTask;
    }

}

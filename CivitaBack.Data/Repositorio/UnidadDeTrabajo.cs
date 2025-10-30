using CivitaBack.Data.EF;
using CivitaBack.Utils;

namespace CivitaBack.Data.Repositorio;

public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    protected readonly AppDbContext _context;

    public UnidadDeTrabajo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync()
    {
        // Esta es la única llamada a SaveChangesAsync en todo el proyecto
        return await _context.SaveChangesAsync();
    }
}
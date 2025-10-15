using CivitaBack.Data.EF;

namespace CivitaBack.Data.Repositorio;

public abstract class GenericoRepositorio
{
    protected readonly AppDbContext _context;

    public GenericoRepositorio(AppDbContext context)
    {
        _context = context;
    }
    
    // Método genérico para guardar cambios de manera asíncrona
    protected async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    
}
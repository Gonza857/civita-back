using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class MisionRepositorio
    : GenericoRepositorio<Mision, MisionEF>, IMisionRepositorio
{

    public MisionRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }
    
    public async Task<Mision?> ObtenerPorId(int id)
    {
        var mision = await _context.Mision
            .Include(m => m.Condicion)
            .Where(m => m.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return base.Mapear<Mision>(mision);
    }

    public async Task<List<Mision>> ObtenerTodasMisiones()
    {
        var misiones = await _context.Mision
            .Where(m => m.Disponible)
            .ToListAsync();
        return base.MapearLista<Mision>(misiones);
    }

    private async Task<List<Mision>> ObtenerMisionesSegunFecha(int idUsuario, DateTime limite)
    {
        var misiones = await _context.MisionPartida
            .Include(mp => mp.Partida)
            .ThenInclude(p => p.Usuario)
            .Where(e => e.Partida.Usuario.Id == idUsuario && e.FechaEntrega > limite)
            .Select(mp => mp.Mision)
            .ToListAsync();
        return base.MapearLista<Mision>(misiones);
    }

    public async Task<List<Mision>> ObtenerMisionesDia(int idUsuario)
    {
        // Doy por sentado que ya le asigné todas las misiones
        DateTime limite = DateTime.UtcNow.AddDays(-1); 
        return await this.ObtenerMisionesSegunFecha(idUsuario, limite);
    }

    public async Task<List<Mision>> ObtenerMisionesSemana(int idUsuario)
    {
        // Doy por sentado que ya le asigné todas las misiones
        DateTime limite = DateTime.UtcNow.AddDays(-7); 
        return await this.ObtenerMisionesSegunFecha(idUsuario, limite);
    }

    public async Task<List<Mision>> ObtenerMisionesMes(int idUsuario)
    {
        // Doy por sentado que ya le asigné todas las misiones
        DateTime limite = DateTime.UtcNow.AddDays(-30); 
        return await this.ObtenerMisionesSegunFecha(idUsuario, limite);
    }

    public async Task<List<Mision>> Listado()
    {
        var misiones = await _context.Mision
            .Include(m => m.Condicion)
            .ThenInclude(c => c.Estructura)
            .ToListAsync();
        return base.MapearLista<Mision>(misiones);
    }

    public async Task<List<Mision>> ListadoActivo()
    {
        var misiones = await _context.Mision
            .Include(m => m.Condicion)
            .Where(m => m.Disponible)
            .ToListAsync();
        return base.MapearLista<Mision>(misiones);
    }
}
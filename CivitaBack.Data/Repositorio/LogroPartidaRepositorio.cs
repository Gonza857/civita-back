using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface ILogroPartidaRepositorio : IRepositorioBase<LogroPartida>
{
    Task<List<Logro>> ObtenerLogrosIncompletos(int partidaId);
    Task<List<Logro>> ObtenerLogrosCompletos(int partidaId);

    Task ReiniciarLogrosPartida(int partidaId);
    Task<List<Logro>> ObtenerLogrosParaReclamarQueNoEstenCumplidos(List<int> idsLogros);

    Task<List<Logro>> ObtenerLogrosNoCumplidos(int idPartida);
}

public class LogroPartidaRepositorio : GenericoRepositorio, ILogroPartidaRepositorio
{
    public LogroPartidaRepositorio(AppDbContext context) : base(context) { }
    
    public async Task<List<Logro>> ObtenerLogrosCompletos(int partidaId)
    {
        //SELECT*
        //FROM Logros l
        //WHERE EXISTS(
        //    SELECT 1
        //    FROM LogroPartida lp
        //    WHERE lp.LogroId = l.Id
        //      AND lp.PartidaId = @partidaId
        //);
        return await _context.Logro
            .Include(l => l.TipoLogro)
            .Where(l => l.LogroPartidas.Any(lp => lp.PartidaId == partidaId))
            .ToListAsync();
    }

    public async Task<List<Logro>> ObtenerLogrosIncompletos(int partidaId)
    {
        //SELECT*
        //FROM Logros l
        //WHERE NOT EXISTS(
        //    SELECT 1
        //    FROM LogroPartida lp
        //    WHERE lp.LogroId = l.Id
        //      AND lp.PartidaId = @partidaId
        //);
        return await _context.Logro
            .Include(l => l.TipoLogro)
            .Where(l => !l.LogroPartidas.Any(lp => lp.PartidaId == partidaId))
            .ToListAsync();

    }

    public async Task ReiniciarLogrosPartida(int partidaId)
    {
        var logros = _context.LogroPartida.Where(e => e.PartidaId == partidaId);
        _context.LogroPartida.RemoveRange(logros);
        await _context.SaveChangesAsync();
    
    public async Task<List<Logro>> ObtenerLogrosParaReclamarQueNoEstenCumplidos(List<int> idsLogros)
    {
        // Traemos los IDs de los logros ya cumplidos
        var logrosCumplidosIds = await _context.LogroPartida
            .Select(lp => lp.LogroId)
            .Distinct()
            .ToListAsync();

        // Devolvemos los logros que están en la lista que me diste,
        // pero que no figuran en LogroPartida
        return await _context.Logro
            .Where(l => idsLogros.Contains(l.Id) && !logrosCumplidosIds.Contains(l.Id))
            .ToListAsync();
    }
    
    public async Task<List<Logro>> ObtenerLogrosNoCumplidos(int partidaId)
    {
        var logrosCumplidosIds = await _context.LogroPartida
            .Where(lp => lp.PartidaId == partidaId)
            .Select(lp => lp.LogroId)
            .ToListAsync();

        var logrosNoCumplidos = await _context.Logro
            .Where(l => !logrosCumplidosIds.Contains(l.Id))
            .ToListAsync();

        return logrosNoCumplidos;
    }

    public Task<LogroPartida?> ObtenerPorId(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<LogroPartida>> ObtenerTodos()
    {
        throw new NotImplementedException();
    }

    public Task Actualizar(LogroPartida entidad)
    {
        throw new NotImplementedException();
    }

    public Task Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public async Task Guardar(LogroPartida entidad)
    {
        entidad.Creado = DateTime.UtcNow;
        await _context.LogroPartida.AddAsync(entidad);
        await base.GuardarCambiosAsync();
    }
}

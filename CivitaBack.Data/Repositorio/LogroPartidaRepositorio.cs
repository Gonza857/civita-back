using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class LogroPartidaRepositorio 
    : GenericoRepositorio<LogroPartida, LogroPartidaEF>, ILogroPartidaRepositorio
{
    public LogroPartidaRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }
    
    public async Task<List<Logro>> ObtenerLogrosCompletos(int partidaId)
    {
        List<LogroEF> logros = await _context.Logro
            .Include(l => l.TipoLogro)
            .Where(l => l.LogroPartidas.Any(lp => lp.PartidaId == partidaId))
            .ToListAsync();
        return base.MapearLista<Logro>(logros);
    }

    public async Task<List<Logro>> ObtenerLogrosIncompletos(int partidaId)
    {
        List<LogroEF> logros = await _context.Logro
            .Include(l => l.TipoLogro)
            .Where(l => !l.LogroPartidas.Any(lp => lp.PartidaId == partidaId))
            .ToListAsync();
        return base.MapearLista<Logro>(logros);

    }

    public async Task ReiniciarLogrosPartida(int partidaId)
    {
        List<LogroPartidaEF> logros = await _context.LogroPartida
            .Where(e => e.PartidaId == partidaId)
            .ToListAsync();

        _context.LogroPartida.RemoveRange(logros);
    }
    
    public async Task<List<Logro>> ObtenerLogrosParaReclamarQueNoEstenCumplidos(List<int> idsLogros)
    {
        // Traemos los IDs de los logros ya cumplidos
        List<int> logrosCumplidosIds = await _context.LogroPartida
            .Select(lp => lp.LogroId)
            .Distinct()
            .ToListAsync();

        // Devolvemos los logros que están en la lista que me diste,
        // pero que no figuran en LogroPartida
        List<LogroEF> logros =  await _context.Logro
            .Where(l => idsLogros.Contains(l.Id) && !logrosCumplidosIds.Contains(l.Id))
            .ToListAsync();
        return base.MapearLista<Logro>(logros);
    }
    
    public async Task<List<Logro>> ObtenerLogrosNoCumplidos(int partidaId)
    {
        List<int> logrosCumplidosIds = await _context.LogroPartida
            .Where(lp => lp.PartidaId == partidaId)
            .Select(lp => lp.LogroId)
            .ToListAsync();

        List<LogroEF> logrosNoCumplidos = await _context.Logro
            .Where(l => !logrosCumplidosIds.Contains(l.Id))
            .ToListAsync();

        return base.MapearLista<Logro>(logrosNoCumplidos);
    }

    public async Task<LogroPartida?> ObtenerPorId(int id)
    {
        var lpEf = await base.ObtenerPorId(id);
        return base.Mapear<LogroPartida>(lpEf);
    }

    public async Task<List<LogroPartida>> ObtenerTodos()
    {
        var listaLogroPartidaEf = await base.ObtenerTodos();
        return base.MapearLista<LogroPartida>(listaLogroPartidaEf);
    }

    public Task Actualizar(LogroPartida entidad)
    {
        this.ActualizarEditado(entidad);
        base.Actualizar(entidad);
        return Task.CompletedTask;
    }

    public Task Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public Task Guardar(LogroPartida entidad)
    {
        throw new NotImplementedException();
    }

    public async Task Agregar(LogroPartida entidad)
    {
        this.ActualizarCreado(entidad);
        var entidadParaGuardar = base.Mapear<LogroPartidaEF>(entidad);
        await _context.LogroPartida.AddAsync(entidadParaGuardar);
    }

    public async Task AgregarVarios(List<LogroPartida> entidades)
    {
        foreach (var e in entidades) this.ActualizarCreado(e);
        await base.AgregarVarios(entidades);
    }
    
    private void ActualizarEditado(LogroPartida entidad) => entidad.Editado = DateTime.UtcNow;
    private void ActualizarCreado(LogroPartida entidad) => entidad.Creado = DateTime.UtcNow;
    
}

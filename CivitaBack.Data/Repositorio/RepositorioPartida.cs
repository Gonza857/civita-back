using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface IRepositorioPartida
{
    Partida ObtenerPorUsuarioId(int IdUsuario);
    Partida CrearPartida(Usuario usuario);
    List<Partida> ObtenerPartidas();
    Task GuardarJsonMapaAsync(int partidaId, string json);
    Task GuardarEstructurasEnMapaAsync(int partidaId, List<EstructuraEnMapa> estructuras);
    Task GuardarEstructurasEnMapaAsync(int partidaId, List<EstructuraEnMapaDTO> estructuras);

    Task ActualizarMapaAsync(int partidaId, string jsonMapa);
    Task ActualizarEstructurasMapaAsync(int partidaId, List<EstructuraEnMapaDTO> estructuras);

    Task<GuardarMapaDTO?> ObtenerMapaAsync(int partidaId);

}

public class RepositorioPartida : IRepositorioPartida
{
    private readonly AppDbContext _context;

    public RepositorioPartida(AppDbContext context)
    {
        _context = context;
    }

    public Partida CrearPartida(Usuario usuario)
    {
        _context.Add(usuario);
        _context.SaveChanges();

        Partida partida = new Partida
        {
            UsuarioId = usuario.Id
        };
        _context.Add(partida);
        _context.SaveChanges();
        return partida;
    }

    public List<Partida> ObtenerPartidas()
    {
        return _context.Partida.ToList();
    }

    public Partida ObtenerPorUsuarioId(int IdUsuario)
    {
        throw new NotImplementedException();
    }

  
    /// Guarda el JSON del mapa completo (snapshot).
    public async Task GuardarJsonMapaAsync(int partidaId, string json)
    {
        var partida = await _context.Partida.FindAsync(partidaId);
        if (partida == null)
            throw new Exception("No se encontró la partida.");

        partida.JsonMapa = json;
        partida.UltimaVez = DateTime.UtcNow;

        _context.Partida.Update(partida);
        await _context.SaveChangesAsync();
    }

    /// Guarda estructuras desde una lista de entidades EstructuraEnMapa (ya mapeadas).
    public async Task GuardarEstructurasEnMapaAsync(int partidaId, List<EstructuraEnMapa> estructuras)
    {
        try
        {
            var existentes = _context.EstructuraEnMapa.Where(e => e.PartidaId == partidaId);
            _context.EstructuraEnMapa.RemoveRange(existentes);

            foreach (var e in estructuras)
            {
                e.PartidaId = partidaId;
                _context.EstructuraEnMapa.Add(e);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error en GuardarEstructurasEnMapaAsync: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"INNER: {ex.InnerException.Message}");
            throw;
        }
    }

    
    /// Guarda estructuras desde DTO (se convierten internamente a entidades).
    public async Task GuardarEstructurasEnMapaAsync(int partidaId, List<EstructuraEnMapaDTO> estructuras)
    {
        var entidades = estructuras.Select(e => new EstructuraEnMapa
        {
            PartidaId = partidaId,
            EstructuraId = e.EstructuraId,
            X = e.X,
            Y = e.Y,
            Width = e.Width,
            Height = e.Height
        }).ToList();

        await GuardarEstructurasEnMapaAsync(partidaId, entidades);
    }

    /// Actualiza el JSON del mapa.
    public async Task ActualizarMapaAsync(int partidaId, string jsonMapa)
    {
        var partida = await _context.Partida.FindAsync(partidaId);
        if (partida == null)
            throw new Exception("No se encontró la partida.");

        partida.JsonMapa = jsonMapa;
        partida.UltimaVez = DateTime.UtcNow;

        _context.Partida.Update(partida);
        await _context.SaveChangesAsync();
    }

   
    /// Actualiza las estructuras del mapa según los DTO recibidos.
    public async Task ActualizarEstructurasMapaAsync(int partidaId, List<EstructuraEnMapaDTO> estructuras)
    {
        try
        {
            var existentes = _context.EstructuraEnMapa.Where(e => e.PartidaId == partidaId);
            _context.EstructuraEnMapa.RemoveRange(existentes);

            var nuevas = estructuras.Select(e => new EstructuraEnMapa
            {
                PartidaId = partidaId,
                EstructuraId = e.EstructuraId,
                X = e.X,
                Y = e.Y,
                Width = e.Width,
                Height = e.Height
            });

            _context.EstructuraEnMapa.AddRange(nuevas);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error al actualizar estructuras: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"INNER: {ex.InnerException.Message}");
            throw;
        }
    }


    public async Task<GuardarMapaDTO?> ObtenerMapaAsync(int partidaId)
    {
        var partida = await _context.Partida
            .Include(p => p.EstructuraEnMapa!)
            .FirstOrDefaultAsync(p => p.Id == partidaId);

        if (partida == null)
            return null;

        var dto = new GuardarMapaDTO
        {
            PartidaId = partida.Id,
            JsonMapa = partida.JsonMapa ?? string.Empty,
            Estructuras = partida.EstructuraEnMapa?
                .Select(e => new EstructuraEnMapaDTO
                {
                    EstructuraId = e.EstructuraId,
                    X = e.X,
                    Y = e.Y,
                    Width = e.Width,
                    Height = e.Height
                })
                .ToList()
        };

        return dto;
    }

}


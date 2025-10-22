using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.EF;
using CivitaBack.Logica.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface IPartidaRepositorio
{
    Task GuardarCambios();
    
    Task<Partida?> ObtenerPorUsuarioId(int IdUsuario);
    Task<Partida> CrearPartida(int idUsuario);
    Task<List<Partida>> ObtenerPartidas();
    void Guardar(Partida partida);
    
    Task<bool> ActualizarMapaAsync(Partida partida);
    
    Task ActualizarEstructurasMapaAsync(int partidaId, List<EstructuraMapaDTO> estructuras);
    Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId);
    Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId);
    Task<List<EstructuraMapa>> ObtenerEstructurasDeUnMapa(int partidaId);
}

    public class PartidaRepositorio : GenericoRepositorio, IPartidaRepositorio
{
        public PartidaRepositorio(AppDbContext context) : base(context) { }
        
        // --------------------------------------------------------------------
        // 🧱 Métodos básicos (compatibles con rama desarrollo)
        // --------------------------------------------------------------------
        public async Task GuardarCambios()
        {
            await base.GuardarCambiosAsync();
        }

        public void Guardar(Partida partida)
        {
            _context.Add(partida);
            _context.SaveChanges();
        }

        public async Task<Partida> CrearPartida(int idUsuario)
        {
         
            var rutaMapa = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "..", "..", "..", "..",                
                "CivitaBack.Data", "DTO", "mapa_base.json"
            );

            rutaMapa = Path.GetFullPath(rutaMapa);
            
            if (!File.Exists(rutaMapa))
                throw new FileNotFoundException("No se encontró el archivo de mapa base.", rutaMapa);

            var contenidoMapa = File.ReadAllText(rutaMapa);

            var partida = new Partida
            {
                UsuarioId = idUsuario,
                JsonMapa = contenidoMapa,
                UltimaVez = DateTime.UtcNow
            };

            await _context.Partida.AddAsync(partida);

            await _context.SaveChangesAsync();

            return partida;
        }
        
        public async Task<List<Partida>> ObtenerPartidas()
        {
            return await _context.Partida
                .Include(p => p.Usuario)
                .Include(p => p.Recursos)
                .ToListAsync();
        }

        public async Task<Partida?> ObtenerPorUsuarioId(int idUsuario)
        {
            return await _context.Partida
                .Include(p => p.EstructuraMapa)
                .Include(p => p.Recursos)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.UsuarioId == idUsuario);
        }
        
        public async Task<bool> ActualizarMapaAsync(Partida partida)
        {
            partida.Editado = DateTime.UtcNow;
            _context.Partida.Update(partida);
            var rowsAfectadas = await _context.SaveChangesAsync();
            return rowsAfectadas > 0;
        }

        public async Task ActualizarEstructurasMapaAsync(int partidaId, List<EstructuraMapaDTO> estructuras)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existentes = _context.EstructuraMapa.Where(e => e.PartidaId == partidaId);
                _context.EstructuraMapa.RemoveRange(existentes);
                await _context.SaveChangesAsync();

                var nuevas = estructuras.Select(e => new EstructuraMapa
                {
                    PartidaId = partidaId,
                    EstructuraId = e.EstructuraId,
                    X = e.X,
                    Y = e.Y,
                    Width = e.Width,
                    Height = e.Height,
                    Editado = DateTime.UtcNow
                });

                await _context.EstructuraMapa.AddRangeAsync(nuevas);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new PersistenciaException("Ocurrió un error al actualizar.");
            }
        }

        public async Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId)
        {
            return await _context.Partida
                .Include(p => p.EstructuraMapa)
                .ThenInclude(em => em.Estructura)
                .FirstOrDefaultAsync(p => p.Id == partidaId);
        }

        public async Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId)
        {
            var partida = await _context.Partida
                .Include(p => p.EstructuraMapa)
                .ThenInclude(em => em.Estructura)
                .FirstOrDefaultAsync(p => p.Id == partidaId);

            if (partida == null)
                throw new Exception("No se encontró la partida.");

            var pathBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "mapa", "mapa_base.json");
            if (!File.Exists(pathBase))
                throw new Exception($"No se encontró el archivo base del mapa en {pathBase}");

            var mapaJson = await File.ReadAllTextAsync(pathBase);
            using var doc = JsonDocument.Parse(mapaJson);

            // 🔹 Capas base (solo tiles, sin objetos)
            var capasBase = doc.RootElement
                .GetProperty("layers")
                .EnumerateArray()
                .Where(l => l.GetProperty("type").GetString() != "objectgroup")
                .Select(l => JsonSerializer.Deserialize<object>(l.GetRawText()))
                .ToList();

            // 🔹 Capas dinámicas a partir de estructuras en la BD
            var estructuras = partida.EstructuraMapa ?? new List<EstructuraMapa>();
            var capasDinamicas = new Dictionary<string, List<object>>();

            foreach (var e in estructuras)
            {
                var tipo = e.Estructura?.Nombre?.ToLower() ?? "desconocido";
                if (!capasDinamicas.ContainsKey(tipo))
                    capasDinamicas[tipo] = new List<object>();

                capasDinamicas[tipo].Add(new
                {
                    id = e.Id,
                    name = $"{tipo}_{e.Id}",
                    type = tipo,
                    x = e.X,
                    y = e.Y + e.Height,
                    width = e.Width,
                    height = e.Height,
                    visible = true
                });
            }

            var capasEstructuras = capasDinamicas.Select(c => new
            {
                name = c.Key,
                type = "objectgroup",
                objects = c.Value
            }).ToList();

            var todasLasCapas = capasBase.Concat(capasEstructuras).ToList();

            var mapaFinal = new
            {
                width = doc.RootElement.GetProperty("width").GetInt32(),
                height = doc.RootElement.GetProperty("height").GetInt32(),
                tilewidth = doc.RootElement.GetProperty("tilewidth").GetInt32(),
                tileheight = doc.RootElement.GetProperty("tileheight").GetInt32(),
                layers = todasLasCapas,
                felicidad_actual = 60
            };

            return JsonSerializer.Serialize(mapaFinal, new JsonSerializerOptions { WriteIndented = true });
        }

    public Task<List<EstructuraMapa>> ObtenerEstructurasDeUnMapa(int partidaId)
    {
        return _context.EstructuraMapa
            .Where(e => e.PartidaId == partidaId)
            .ToListAsync();    
    }
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio
{
    public interface IRepositorioPartida
    {
        // 🧱 Métodos básicos (de la rama desarrollo)
        Partida ObtenerPorUsuarioId(int IdUsuario);
        Partida CrearPartida(int idUsuario);
        List<Partida> ObtenerPartidas();
        void Guardar(Partida partida);
        void Actualizar();

        // 🧠 Métodos asincrónicos (de tu rama)
        Task ActualizarMapaAsync(int partidaId, string jsonMapa);
        Task ActualizarEstructurasMapaAsync(int partidaId, List<EstructuraEnMapaDTO> estructuras);
        Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId);
        Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId);
    }

    public class PartidaRepositorio : IRepositorioPartida
    {
        private readonly AppDbContext _context;

        public PartidaRepositorio(AppDbContext context)
        {
            _context = context;
        }

        // --------------------------------------------------------------------
        // 🧱 Métodos básicos (compatibles con rama desarrollo)
        // --------------------------------------------------------------------
        public void Actualizar()
        {
            _context.SaveChanges();
        }

        public void Guardar(Partida partida)
        {
            _context.Add(partida);
            _context.SaveChanges();
        }

        public Partida CrearPartida(int idUsuario)
        {
            var partida = new Partida
            {
                UsuarioId = idUsuario,
                UltimaVez = DateTime.UtcNow
            };
            _context.Partida.Add(partida);
            _context.SaveChanges();
            return partida;
        }

        public List<Partida> ObtenerPartidas()
        {
            return _context.Partida
                .Include(p => p.Usuario)
                .Include(p => p.Recursos)
                .ToList();
        }

        public Partida ObtenerPorUsuarioId(int IdUsuario)
        {
            return _context.Partida
                .Include(p => p.Recursos)
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.UsuarioId == IdUsuario);
        }

        // --------------------------------------------------------------------
        // 🧠 Métodos asincrónicos del mapa
        // --------------------------------------------------------------------
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

        public async Task ActualizarEstructurasMapaAsync(int partidaId, List<EstructuraEnMapaDTO> estructuras)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existentes = _context.EstructuraEnMapa.Where(e => e.PartidaId == partidaId);
                _context.EstructuraEnMapa.RemoveRange(existentes);
                await _context.SaveChangesAsync();

                var nuevas = estructuras.Select(e => new EstructuraEnMapa
                {
                    PartidaId = partidaId,
                    EstructuraId = e.EstructuraId,
                    X = e.X,
                    Y = e.Y,
                    Width = e.Width,
                    Height = e.Height
                });

                await _context.EstructuraEnMapa.AddRangeAsync(nuevas);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId)
        {
            return await _context.Partida
                .Include(p => p.EstructuraEnMapa)
                .ThenInclude(em => em.Estructura)
                .FirstOrDefaultAsync(p => p.Id == partidaId);
        }

        public async Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId)
        {
            var partida = await _context.Partida
                .Include(p => p.EstructuraEnMapa)
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
            var estructuras = partida.EstructuraEnMapa ?? new List<EstructuraEnMapa>();
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
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;

namespace CivitaBack.Data.Repositorio
{
    public interface IRepositorioPartida
    {
        Partida ObtenerPorUsuarioId(int IdUsuario);
        Partida CrearPartida(Usuario usuario);
        List<Partida> ObtenerPartidas();

        // 🆕 Métodos asincrónicos
        Task ActualizarMapaAsync(int partidaId, string jsonMapa);
        Task ActualizarEstructurasMapaAsync(int partidaId, List<EstructuraEnMapaDTO> estructuras);
        Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId);
        Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId);
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
                UsuarioId = usuario.Id,
                UltimaVez = DateTime.UtcNow
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

        // --------------------------------------------------------------------
        // 🧠 ACTUALIZAR MAPA Y ESTRUCTURAS
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


        // --------------------------------------------------------------------
        // 🧱 OBTENER MAPA COMPLETO (CONSTRUCTIVO)
        // --------------------------------------------------------------------
        public async Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId)
        {
            return await _context.Partida
                .Include(p => p.EstructuraEnMapa)
                .FirstOrDefaultAsync(p => p.Id == partidaId);
        }

        public async Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId)
        {
            // 1️⃣ Buscar la partida y sus estructuras
            var partida = await _context.Partida
                .Include(p => p.EstructuraEnMapa)
                .ThenInclude(em => em.Estructura)
                .FirstOrDefaultAsync(p => p.Id == partidaId);

            if (partida == null)
                throw new Exception("No se encontró la partida.");

            // 2️⃣ Leer el mapa base desde /wwwroot/assets/mapa/mapa_base.json
            var pathBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "mapa", "mapa_base.json");
            if (!File.Exists(pathBase))
                throw new Exception($"No se encontró el archivo base del mapa en {pathBase}");

            var mapaJson = await File.ReadAllTextAsync(pathBase);
            using var doc = JsonDocument.Parse(mapaJson);

            // 🔸 Tomar las capas base (ej: mapa, calle, decoraciones, etc.)
            var capasBase = doc.RootElement
                .GetProperty("layers")
                .EnumerateArray()
                .Where(l => l.GetProperty("type").GetString() != "objectgroup") // solo capas de tiles
                .Select(l => JsonSerializer.Deserialize<object>(l.GetRawText()))
                .ToList();

            // 3️⃣ Construir las capas dinámicas desde EstructuraEnMapa
            var estructuras = partida.EstructuraEnMapa ?? new List<EstructuraEnMapa>();
            var capasDinamicas = new Dictionary<string, List<object>>();

            foreach (var e in estructuras)
            {
                // 🔸 Usar Nombre de Estructura para el tipo (coincide con Phaser: casa, fabrica, etc.)
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

            // 🔸 Convertir cada tipo de estructura a una capa Tiled
            var capasEstructuras = capasDinamicas.Select(c => new
            {
                name = c.Key,
                type = "objectgroup",
                objects = c.Value
            }).ToList();

            // 4️⃣ Unir las capas base + capas de estructuras
            var todasLasCapas = capasBase.Concat(capasEstructuras).ToList();

            // 5️⃣ Construir el JSON final con metadatos del mapa base
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

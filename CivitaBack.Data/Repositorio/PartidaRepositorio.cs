using System.Text.Json;
using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class PartidaRepositorio
    : GenericoRepositorio<Partida, PartidaEF>, IPartidaRepositorio
{
    public PartidaRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }
    

    public async Task<Partida> CrearPartida(int idUsuario)
    {
        var rutaMapa = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", "..",
            "CivitaBack.Data", "Mapa", "mapa3.json"
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

        await base.Agregar(partida);
        await _context.SaveChangesAsync();
        return partida;
    }

    public async Task<Partida?> ObtenerPorId(int id)
    {
        var partida = await base.ObtenerPorId(e => e.Id == id);
        return base.Mapear<Partida>(partida);
    }

    public override async Task<List<Partida>> ObtenerTodos()
    {
        var partidas = await _context.Partida
            .Include(p => p.Usuario)
            .Include(p => p.Recursos)
            .ToListAsync();
        return base.MapearLista<Partida>(partidas);
    }
    

    public async Task<Partida?> ObtenerPorUsuarioId(int idUsuario)
    {
        var partida =  await _context.Partida
            .Include(p => p.EstructuraMapa)
            .Include(p => p.Recursos)
            .Include(p => p.Usuario)
            .FirstOrDefaultAsync(p => p.UsuarioId == idUsuario);
        return base.Mapear<Partida>(partida);
    }

    public async Task<bool> ActualizarMapaAsync(Partida partida)
    {
        await base.Actualizar(partida);
        return 1 > 0;
    }
    

    public async Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId)
    {
        var partida = await _context.Partida
            .Include(p => p.EstructuraMapa)
            .ThenInclude(em => em.Estructura)
            .FirstOrDefaultAsync(p => p.Id == partidaId);
        return base.Mapear<Partida>(partida);
    }

    public async Task<string> ObtenerMapaJsonPorPartidaIdAsync(int partidaId)
    {
        var partidaEf = await _context.Partida
            .Include(p => p.EstructuraMapa)
            .ThenInclude(em => em.Estructura)
            .FirstOrDefaultAsync(p => p.Id == partidaId);
        var partida = base.Mapear<Partida>(partidaEf);

        if (partida == null)
            throw new Exception("No se encontró la partida.");

        var pathBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "mapa", "mapa3.json");
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

    public async Task<List<EstructuraMapa>> ObtenerEstructurasDeUnMapa(int partidaId)
    {
        var estructuraMapaEf = await _context.EstructuraMapa
            .Where(e => e.PartidaId == partidaId)
            .ToListAsync();
        return base.MapearLista<EstructuraMapa>(estructuraMapaEf);
    }
    

    public async Task<List<Partida>> ObtenerTodasConEstructurasYRecursosAsync()
    {
        var listaPartidasEF = await _dbSet
             .Include(p => p.Recursos)
             .Include(p => p.EstructuraMapa)
             .ThenInclude(em => em.Estructura)
             .ThenInclude(e => e.TipoEstructura)
             .AsNoTracking()
             .ToListAsync();

        return MapearLista<Partida>(listaPartidasEF);
    }
}
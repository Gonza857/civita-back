using System.Text.Json;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Utils;

public class LectorMapa
{
    public static async Task<string> ObtenerMapaFinal(List<EstructuraMapa> estructuras)
    {
        string baseDirectorio = AppContext.BaseDirectory;
        var pathBase = Path.Combine(baseDirectorio, "Mapa", "MapaJuego.json");
        
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
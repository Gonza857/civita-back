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


    public async Task<Partida?> ObtenerPorUsuarioCorreo(string correo)
    {
        var partida = await _context.Partida
            .Include(p => p.Usuario)
            .Where(p => p.Usuario.Mail == correo)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return base.Mapear<Partida>(partida);

    }

    public async Task<Partida> CrearPartida(int idUsuario)
    {
        var rutaMapa = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", "..",
            "CivitaBack.Data", "Mapa", "MapaJuego.json"
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
        return partida;
    }

    public async Task<Partida?> ObtenerPorId(int id)
    {
        var partida = await _context.Partida
            .Include(p => p.Recursos)
            .Include(p => p.EstructuraMapa)
            .Where(p => p.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
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
        var partida = await _context.Partida
            .Include(p => p.EstructuraMapa)
            .Include(p => p.Recursos)
            .Include(p => p.Usuario)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UsuarioId == idUsuario);
        return base.Mapear<Partida>(partida);
    }

    public async Task<bool> ActualizarMapaAsync(Partida partida)
    {
        await base.Actualizar(partida);
        return 1 > 0;
    }
    
    public void SincronizarCambios(Partida partidaDominio)
    {
        var partidaEf = _context.Partida.Local
            .FirstOrDefault(p => p.Id == partidaDominio.Id);

        if (partidaEf != null)
        {
            _mapper.Map(partidaDominio, partidaEf); 
        }
    }

    public async Task<Partida?> ObtenerPorIdTrackeada(int idPartida)
    {
        var partidaEf = await _context.Partida
            .Include(p => p.Recursos)
            .FirstOrDefaultAsync(p => p.Id == idPartida);
        return base.Mapear<Partida?>(partidaEf);
    }
    
    public async Task<Partida?> ObtenerPartidaConMapaAsync(int partidaId)
    {
        var partida = await _context.Partida
            .Include(p => p.EstructuraMapa)
            .FirstOrDefaultAsync(p => p.Id == partidaId);
        return base.Mapear<Partida>(partida);
    }

    public async Task<Partida?> ObtenerPartidaConEstructuras(int partidaId)
    {
        var partidaEf = await _context.Partida
            .Include(p => p.EstructuraMapa)
            .ThenInclude(em => em.Estructura)
            .FirstOrDefaultAsync(p => p.Id == partidaId);
        return base.Mapear<Partida>(partidaEf);
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
             .AsNoTracking()
             .Include(p => p.Recursos)
             .Include(p => p.EstructuraMapa)
             .ThenInclude(em => em.Estructura)
             .ThenInclude(e => e.TipoEstructura)
             .ToListAsync();

        return MapearLista<Partida>(listaPartidasEF);
    }
}
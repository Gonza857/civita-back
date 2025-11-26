using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class MisionPartidaRepositorio
    : GenericoRepositorio<MisionPartida, MisionPartidaEF>, IMisionPartidaRepositorio
{
    public MisionPartidaRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

    public Task<MisionPartida?> ObtenerPorId(int id)
    {
        throw new NotImplementedException();
    }

    public Task MarcarCompletada(MisionPartida mp)
    {
        var mpEF = _mapper.Map<MisionPartidaEF>(mp);
        _context.MisionPartida.Attach(mpEF);
        _context.Entry(mpEF).Property(e => e.FechaCompletado).IsModified = true;
        _context.Entry(mpEF).Property(e => e.Reclamado).IsModified = true;
        return Task.CompletedTask;
    }

    public List<MisionPartida> AgregarMisionesPartida(List<Mision> misiones, Partida partida)
    {
        var misionesPartida = new List<MisionPartida>();
        foreach (var mision in misiones)
        {
            var nuevaAsignacion = new MisionPartida
            {
                MisionId = mision.Id,
                FechaEntrega = DateTime.UtcNow,
                Reclamado = false 
            };

            if (partida.Id == 0)
            {
                nuevaAsignacion.Partida = partida;
            }
            else
            {
                nuevaAsignacion.PartidaId = partida.Id;
            }
            
            misionesPartida.Add(nuevaAsignacion);
        }

        return misionesPartida;
    }

    public void GuardarMisionesPartida(Partida partida)
    {
        base.AgregarVarios(partida.MisionPartidas);
    }

    public async Task<List<MisionPartida>> ObtenerMisionesPartida(int idPartida)
    {
        var misiones = await _context.MisionPartida
            .Include(mp => mp.Mision)
            .ThenInclude(m => m.Condicion)
            .AsNoTracking()
            .Where(mp => mp.PartidaId == idPartida && mp.Mision.Disponible)
            .ToListAsync();
        return base.MapearLista<MisionPartida>(misiones);

    }

    public async Task<MisionPartida> ObtenerUnaMisionDePartida(int idPartida, int idMision)
    {
        var misionPartida = await this.ObtenerMisionDePartidaBase(idPartida, idMision);
        return base.Mapear<MisionPartida>(misionPartida);
    }

    public async Task<List<MisionPartida>> ObtenerMisionesAsignadas(int idPartida)
    {
        var mpList = await _context.MisionPartida
            .Include(mp => mp.Mision)
            .AsNoTracking()
            .Where(mp => mp.PartidaId == idPartida)
            .ToListAsync();
        return base.MapearLista<MisionPartida>(mpList);
    }

    public async Task<Mision> ObtenerMisionPartidaPorId(int idPartida, int idMision)
    {
        var misionPartida = await this.ObtenerMisionDePartidaBase(idPartida, idMision);
        return base.Mapear<Mision>(misionPartida.Mision);
    }

    public async Task<List<MisionPartida>> Listado()
    {
        return await base.ObtenerTodos();
    }

    public async Task<List<MisionPartida>> ListadoPorTipo(TipoMision tipoMision)
    {
        var misiones = await _context.MisionPartida
            .Include(mp => mp.Mision)
            .Where(mp => mp.Mision.Tipo == tipoMision)
            .ToListAsync();
        return base.MapearLista<MisionPartida>(misiones);
    }

    private async Task<MisionPartidaEF> ObtenerMisionDePartidaBase(int idPartida, int idMision)
    {
        return await _context.MisionPartida
            .Include(mp => mp.Mision)
            .ThenInclude(m => m.Condicion)
            .ThenInclude(c => c.Recompensas)
            .Where(mp => mp.PartidaId == idPartida && mp.MisionId == idMision)
            .AsNoTracking()
            .FirstOrDefaultAsync();
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
}
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

    public async Task AgregarMisionesPartida(List<Mision> misiones, Partida partida)
    {
        var misionesPartida = new List<MisionPartida>();
        foreach (var mision in misiones)
        {
            var nuevaAsignacion = new MisionPartida
            {
                PartidaId = partida.Id,
                MisionId = mision.Id,
                FechaEntrega = DateTime.UtcNow,
                Reclamado = false 
            };
            
            misionesPartida.Add(nuevaAsignacion);
        }
        
        await base.AgregarVarios(misionesPartida);
    }

    public async Task<List<MisionPartida>> ObtenerMisionesPartida(int idPartida)
    {
        var misiones = await _context.MisionPartida
            .Include(mp => mp.Mision)
            .AsNoTracking()
            .Where(mp => mp.PartidaId == idPartida && mp.Mision.Disponible)
            .ToListAsync();
        return base.MapearLista<MisionPartida>(misiones);

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
}
using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Data.Migrations;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio
{
    public class EventoRepositorio : GenericoRepositorio<Evento,EventoEF>, IEventoRepositorio
    {
        public EventoRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<EventoMaestro?> ObtenerEventoMaestroAsync()
        {           
            var eventoMaestroEF = await _context.EventoMaestro
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return Mapear<EventoMaestro>(eventoMaestroEF);
        }

        public async Task<Evento> CrearEventoAsync(Evento evento)
        {
            var eventoEF = Mapear<EventoEF>(evento);

            await _dbSet.AddAsync(eventoEF);

            await _context.SaveChangesAsync();

            return Mapear<Evento>(eventoEF);
        }

        public async Task<Evento?> ObtenerEventoConPartidaAsync(int eventoId)
        {
            var eventoEF = await _dbSet
                .Include(e => e.EventoMaestro).ThenInclude(em => em.Efectos)
                 .Include(e => e.Partida).ThenInclude(p => p.Recursos)
                 .Where(e => e.Id == eventoId)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();

            return Mapear<Evento>(eventoEF);
        }

        public async Task<Evento?> ObtenerPorId(int id)
        {
            var evento = await _dbSet
            .Include(e => e.EventoMaestro)
            .Where(e => e.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
            return evento == null ? null : base.Mapear<Evento>(evento);
        }
    }
}

using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
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

        public async Task CrearEventoAsync(Evento evento)
        {
            await base.Agregar(evento);
        }

        public async Task<Evento?> ObtenerEventoConPartidaAsync(int eventoId)
        {
            var eventoEF = await _context.Evento 
                 .Include(e => e.Partida).ThenInclude(p => p.Recursos)
                 .Where(e => e.Id == eventoId)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();

            return Mapear<Evento>(eventoEF);
        }
    }
}

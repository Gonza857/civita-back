using CivitaBack.Domain.Entities;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio
{
    public class EventoRepositorio : GenericoRepositorio, IEventoRepositorio
    {
        public EventoRepositorio(AppDbContext context) : base(context) { }

        public async Task<EventoMaestro?> ObtenerEventoMaestroAsync()
        {
            return await _context.EventoMaestro.FirstOrDefaultAsync();
        }

        public async Task CrearEventoAsync(Evento evento)
        {
            await _context.Evento.AddAsync(evento);
        }

        public async Task<Evento?> ObtenerEventoConPartidaAsync(int eventoId)
        {
            return await _context.Evento
                    .Include(e => e.Partida).ThenInclude(p => p.Recursos)
                    .FirstOrDefaultAsync(e => e.Id == eventoId);
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

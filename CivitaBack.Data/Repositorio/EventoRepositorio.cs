using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.Repositorio
{

    public interface IEventoRepositorio
    {
        Task<EventoMaestro?> ObtenerEventoMaestroAsync();
        Task CrearEventoAsync(Evento evento);
        Task<Evento?> ObtenerEventoConPartidaAsync(int eventoId);
        Task GuardarCambiosAsync();
    }
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

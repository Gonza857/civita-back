using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Logica
{
    public interface IEventoLogica
    {
        Task<EventoDisparadoDTO> DispararEventoAsync(int idPartida);
        Task<EventoResueltoDTO> ResolverEventoAsync(int eventoId, bool aceptado);
    }
    public class EventoLogica : IEventoLogica
    {
        private readonly IEventoRepositorio _eventoRepositorio;
        private readonly IHubContext<EventoHub> _hubContext;

        public EventoLogica(IEventoRepositorio eventoRepositorio, IHubContext<EventoHub> hubContext)
        {
            _eventoRepositorio = eventoRepositorio;
            _hubContext = hubContext;
        }

        public async Task<EventoDisparadoDTO> DispararEventoAsync(int idPartida)
        {
            var maestro = await _eventoRepositorio.ObtenerEventoMaestroAsync(); // Por ahora traigo el primero
            if (maestro == null) return null;

            var evento = new Evento
            {
                PartidaId = idPartida,
                EventoMaestroId = maestro.Id,
                TextoDescripcion = maestro.TextoDescripcion,
                TextoAceptar = maestro.TextoAceptar,
                TextoRechazar = maestro.TextoRechazar,
                EcoCoinsAceptar = maestro.EcoCoinsAceptar,
                FelicidadAceptar = maestro.FelicidadAceptar,
                ContaminacionAceptar = maestro.ContaminacionAceptar,
                FelicidadRechazar = maestro.FelicidadRechazar,
                ContaminacionRechazar = maestro.ContaminacionRechazar,
                SeDisparo = true,
                Resuelto = false
            };

            await _eventoRepositorio.CrearEventoAsync(evento);
            await _eventoRepositorio.GuardarCambiosAsync();

            await _hubContext.Clients.Group(idPartida.ToString())
                .SendAsync("EventoDisparado", new EventoDisparadoDTO
                {
                    Id = evento.Id,
                    Titulo = maestro.Nombre,
                    TextoDescripcion = maestro.TextoDescripcion,
                    EcoCoinsAceptar = maestro.EcoCoinsAceptar,
                    FelicidadAceptar = maestro.FelicidadAceptar,
                    ContaminacionAceptar = maestro.ContaminacionAceptar,
                    FelicidadRechazar = maestro.FelicidadRechazar,
                    ContaminacionRechazar = maestro.ContaminacionRechazar,
                });

            return new EventoDisparadoDTO
            {
                Id = evento.Id,
                Titulo = maestro.Nombre,
                TextoDescripcion = maestro.TextoDescripcion,
                EcoCoinsAceptar = maestro.EcoCoinsAceptar,
                FelicidadAceptar = maestro.FelicidadAceptar,
                ContaminacionAceptar = maestro.ContaminacionAceptar,
                FelicidadRechazar = maestro.FelicidadRechazar,
                ContaminacionRechazar = maestro.ContaminacionRechazar,
            };

        }

        public async Task<EventoResueltoDTO> ResolverEventoAsync(int eventoId, bool acepto)
        {
            var evento = await _eventoRepositorio.ObtenerEventoConPartidaAsync(eventoId);
            if (evento == null || evento.Resuelto) throw new Exception($"No se encontró el evento {eventoId}");

            var partida = evento.Partida;
            if (partida == null) throw new Exception("Evento sin partida asociada — estado inválido");

            // Aplicar efectos según decisión
            partida.Recursos.EcoCoins = Math.Max(0, partida.Recursos.EcoCoins + (acepto ? evento.EcoCoinsAceptar : 0));
            partida.Recursos.Felicidad = Math.Clamp(partida.Recursos.Felicidad + (acepto ? evento.FelicidadAceptar : evento.FelicidadRechazar), 0, 100);
            partida.Recursos.Contaminacion = Math.Clamp(partida.Recursos.Contaminacion + (acepto ? evento.ContaminacionAceptar : evento.ContaminacionRechazar), 0, 100);

            evento.Resuelto = true;

            await _eventoRepositorio.GuardarCambiosAsync();

            await _hubContext.Clients.Group(evento.PartidaId.ToString())
                .SendAsync("EventoResuelto", new EventoResueltoDTO { Id = evento.Id, Texto = acepto ? evento.TextoAceptar : evento.TextoRechazar });

            return new EventoResueltoDTO { Id = evento.Id, Texto = acepto ? evento.TextoAceptar : evento.TextoRechazar };
        }

    }
}

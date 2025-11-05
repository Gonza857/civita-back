using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

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
        private readonly IUnidadDeTrabajo _uow;
        private readonly IMapper _mapper;

        public EventoLogica(IEventoRepositorio eventoRepositorio, IUnidadDeTrabajo uow, IMapper mapper)
        {
            _eventoRepositorio = eventoRepositorio;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<EventoDisparadoDTO> DispararEventoAsync(int idPartida)
        {
            var maestro = await _eventoRepositorio.ObtenerEventoMaestroAsync();
            if (maestro == null) return null;

            var evento = _mapper.Map<Evento>(maestro); 

            evento.EventoMaestroId = maestro.Id;
            evento.EventoMaestro = null; 
            evento.PartidaId = idPartida;
            evento.SeDisparo = true;
            evento.Resuelto = false;

            var eventoCreado = await _eventoRepositorio.CrearEventoAsync(evento);

            eventoCreado.EventoMaestro = maestro;

            var respuestaDTO = _mapper.Map<EventoDisparadoDTO>(eventoCreado);

            return respuestaDTO;
        }

        public async Task<EventoResueltoDTO> ResolverEventoAsync(int eventoId, bool acepto)
        {
            var evento = await _eventoRepositorio.ObtenerEventoConPartidaAsync(eventoId);
            if (evento == null || evento.Resuelto) throw new Exception($"No se encontró el evento {eventoId}");

            var partida = evento.Partida;
            if (partida == null) throw new Exception("Evento sin partida asociada — estado inválido");

            // Aplicar efectos según decisión
            partida.Recursos.EcoCoins = Math.Max(0, partida.Recursos.EcoCoins + (acepto ? evento.EcoCoinsAceptar : 0));
            partida.Recursos.Felicidad =
                Math.Clamp(partida.Recursos.Felicidad + (acepto ? evento.FelicidadAceptar : evento.FelicidadRechazar),
                    0, 100);
            partida.Recursos.Contaminacion =
                Math.Clamp(
                    partida.Recursos.Contaminacion +
                    (acepto ? evento.ContaminacionAceptar : evento.ContaminacionRechazar), 0, 100);

            evento.Resuelto = true;

            await this._uow.CommitAsync();

            var resultadoDTO = _mapper.Map<EventoResueltoDTO>(
        evento,
        opt => opt.Items.Add("Aceptado", acepto)
    );

            return resultadoDTO;
        }
    }
}
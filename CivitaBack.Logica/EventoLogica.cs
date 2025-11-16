using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{

    public class EventoLogica : IEventoLogica
    {
        private readonly IEventoRepositorio _eventoRepositorio;
        private readonly IUnidadDeTrabajo _uow;
        private readonly IMapper _mapper;
        private readonly IActualizarRecursosLogica _actualizarRecursosLogica;
        private readonly IAccesoUsuarios _accesoUsuarios;

        public EventoLogica(IEventoRepositorio eventoRepositorio, IAccesoUsuarios accesoUsuarios, IUnidadDeTrabajo uow, 
            IMapper mapper, IActualizarRecursosLogica actualizarRecursosLogica)
        {
            _eventoRepositorio = eventoRepositorio;
            _accesoUsuarios = accesoUsuarios;
            _uow = uow;
            _mapper = mapper;
            _actualizarRecursosLogica = actualizarRecursosLogica;
        }

        public async Task<EventoDisparadoDTO> DispararEventoAsync(int idPartida)
        {
            var maestro = await _eventoRepositorio.ObtenerEventoMaestroAsync();
            if (maestro == null) return null;

            var evento = _mapper.Map<Evento>(maestro);


            if (evento.Partida != null)
            {
                _accesoUsuarios.ValidarAcceso(evento.Partida.UsuarioId);
            } else
            {
                throw new PartidaExcepcion("Partida no encontrada");
            }

            await _eventoRepositorio.CrearEventoAsync(evento);
            await this._uow.CommitAsync();

            evento.EventoMaestro = maestro;
            evento.SeDisparo = true;

            var respuestaDTO = _mapper.Map<EventoDisparadoDTO>(evento);

            return respuestaDTO;
        }

        public async Task<EventoResueltoDTO> ResolverEventoPreguntaAsync(int eventoId, string respuestaElegida)
        {
            var evento = await _eventoRepositorio.ObtenerEventoConPartidaAsync(eventoId);

            if (evento == null || evento.Resuelto) throw new EventoException("Evento no encontrado o ya resuelto.");
            var partida = evento.Partida;
            if (partida == null) throw new PartidaExcepcion("Evento sin partida asociada.");

            _accesoUsuarios.ValidarAcceso(partida.UsuarioId);

            bool esCorrecta = (respuestaElegida == evento.EventoMaestro.RespuestaCorrecta);
            TipoResultado tipoResultado = esCorrecta ? TipoResultado.ACIERTO : TipoResultado.FALLO;

            var efectoAplicable = evento.EventoMaestro.Efectos?
                .FirstOrDefault(e => e.TipoResultado == tipoResultado);

            if (efectoAplicable == null) throw new EventoException("Configuración de efectos faltante.");

            _actualizarRecursosLogica.ActualizarRecursosAsync(
                partida,
                efectoAplicable.Felicidad,
                efectoAplicable.Contaminacion,
                efectoAplicable.EcoCoins,
                efectoAplicable.Energia 
            );

            evento.Resuelto = true;
            evento.RespuestaJugador = respuestaElegida;
            evento.EcoCoinsAplicada = efectoAplicable.EcoCoins;
            evento.FelicidadAplicada = efectoAplicable.Felicidad;
            evento.ContaminacionAplicada = efectoAplicable.Contaminacion;
            evento.EnergiaAplicada = efectoAplicable.Energia;
            evento.ExperienciaAplicada = efectoAplicable.Experiencia;

            await _eventoRepositorio.Actualizar(evento);
            await this._uow.CommitAsync();

            var mensajeFinal = esCorrecta ? "¡Respuesta correcta! Recompensas aplicadas." : "Respuesta incorrecta. Penalización aplicada.";

            return new EventoResueltoDTO
            {
                Id = evento.Id,
                TextoRespuesta = mensajeFinal, 
                PartidaId = partida.Id
            };
        }
    }
}
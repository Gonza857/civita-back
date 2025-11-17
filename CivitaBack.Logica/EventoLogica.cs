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
        private readonly IActualizarRecursosLogica _actualizarRecursosLogica;
        private readonly IPartidaRepositorio _partidaRepositorio;

        public EventoLogica(IEventoRepositorio eventoRepositorio, IUnidadDeTrabajo uow, 
            IActualizarRecursosLogica actualizarRecursosLogica, IPartidaRepositorio partidaRepositorio  )
        {
            _eventoRepositorio = eventoRepositorio;
            _uow = uow;
            _actualizarRecursosLogica = actualizarRecursosLogica;
            _partidaRepositorio = partidaRepositorio;
        }

        public async Task<EventoDisparadoDTO> DispararEventoAsync(int idPartida)
        {
            var maestro = await _eventoRepositorio.ObtenerEventoMaestroAsync();
            if (maestro == null) throw new EventoException("Evento maestro no encontrado.");

            var evento = new Evento
            {
                PartidaId = idPartida,
                EventoMaestroId = maestro.Id,
                Contenido = maestro.ContenidoPrincipal,
                SeDisparo = true,
                Resuelto = false
            };

            evento = await _eventoRepositorio.CrearEventoAsync(evento);

            var dto = new EventoDisparadoDTO
            {
                Id = evento.Id,
                TipoEvento = maestro.TipoEvento.ToString(),
                Titulo = maestro.Titulo,
                PreguntaTexto = maestro.ContenidoPrincipal,
                OpcionA_Texto = maestro.OpcionA_Texto,
                OpcionB_Texto = maestro.OpcionB_Texto,
                EfectoAciertoResumen = FormatoEfectos(maestro.Efectos?.FirstOrDefault(e => e.TipoResultado == TipoResultado.ACIERTO))
            };

            return dto;
        }

        public async Task<EventoResueltoDTO> ResolverEventoPreguntaAsync(int eventoId, string respuestaElegida)
        {
            var evento = await _eventoRepositorio.ObtenerEventoConPartidaAsync(eventoId);

            if (evento == null || evento.Resuelto) throw new EventoException("Evento no encontrado o ya resuelto.");
            var partida = evento.Partida;
            if (partida == null) throw new PartidaExcepcion("Evento sin partida asociada.");

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

        public static string FormatoEfectos(EfectoEvento? ef)
        {
            if (ef == null) return string.Empty;
            return $"+{ef.EcoCoins} EcoCoins, +{ef.Felicidad} Felicidad, {ef.Contaminacion} Contaminación, +{ef.Energia} Energía";
        }
    }
}
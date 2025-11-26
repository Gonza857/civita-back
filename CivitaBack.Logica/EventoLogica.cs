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
        private readonly IMapper _mapper;

        public EventoLogica(IEventoRepositorio eventoRepositorio, IUnidadDeTrabajo uow, 
            IActualizarRecursosLogica actualizarRecursosLogica, IPartidaRepositorio partidaRepositorio, IMapper mapper  )
        {
            _eventoRepositorio = eventoRepositorio;
            _uow = uow;
            _actualizarRecursosLogica = actualizarRecursosLogica;
            _partidaRepositorio = partidaRepositorio;
            _mapper = mapper;
        }

        public async Task<EventoDisparadoDTO> DispararEventoAsync(int idPartida)
        {
            const int EVENTO_PREGUNTA = 1;

            var maestro = await _eventoRepositorio.ObtenerEventoMaestroAsync(EVENTO_PREGUNTA);
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
                RespuestaCorrecta = maestro.RespuestaCorrecta,
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

            evento.Partida = null;

            await _eventoRepositorio.Actualizar(evento);

            if (partida.Recursos != null)
            {
                partida.Recursos.Partida = null; 
            }
            partida.Usuario = null; 
            partida.Evento = null;

            await _partidaRepositorio.Actualizar(partida);

            await this._uow.CommitAsync();

            var mensajeFinal = esCorrecta ? "¡Respuesta correcta! Recompensas aplicadas." : "Respuesta incorrecta. Penalización aplicada.";

            var recursosDTO = _mapper.Map<RecursoDTO>(partida.Recursos);

            return new EventoResueltoDTO
            {
                Id = evento.Id,
                TextoRespuesta = mensajeFinal,
                RecursosAplicados = FormatoEfectos(efectoAplicable),
                PartidaId = partida.Id,
                RecursosActualizados = recursosDTO
            };
        }

        public async Task<EventoDisparadoDTO> DispararEventoInformativoAsync(Partida partida, int tipId)
        {
            bool yaEnviado = await _eventoRepositorio.ExisteTipEnviadoAsync(partida.Id, tipId);

            if (yaEnviado) return null; 

            var maestroTip = await _eventoRepositorio.ObtenerEventoMaestroAsync(tipId);
            if (maestroTip == null) throw new EventoException("Evento maestro no encontrado.");

            var evento = new Evento
            {
                PartidaId = partida.Id,
                EventoMaestroId = maestroTip.Id,
                Contenido = maestroTip.ContenidoPrincipal,
                SeDisparo = true,
                Resuelto = true, 
            };

            var dto = new EventoDisparadoDTO
            {
                Id = evento.Id,
                TipoEvento = maestroTip.TipoEvento.ToString(),
                Titulo = maestroTip.Titulo,
                PreguntaTexto = maestroTip.ContenidoPrincipal,
                OpcionA_Texto = maestroTip.OpcionA_Texto,
                OpcionB_Texto = "",
                RespuestaCorrecta = maestroTip.RespuestaCorrecta,
                EfectoAciertoResumen = ""
            };

            await _eventoRepositorio.CrearEventoAsync(evento);

            return dto;
        }

        public static string FormatoEfectos(EfectoEvento? ef)
        {
            if (ef == null) return string.Empty;

            string Format(string nombre, int valor)
            {
                string signo = valor > 0 ? "+" : "";
                return $"{signo}{valor} {nombre}";
            }

            return string.Join(", ", new[]
            {
            Format("EcoCoins", ef.EcoCoins),
            Format("Felicidad", ef.Felicidad),
            Format("Contaminación", ef.Contaminacion),
            Format("Energía", ef.Energia)
            });
        }
    }
}
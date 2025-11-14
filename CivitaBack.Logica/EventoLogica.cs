using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica
{

    public class EventoLogica : IEventoLogica
    {
        private readonly IEventoRepositorio _eventoRepositorio;
        private readonly IPartidaRepositorio _partidaRepositorio;
        private readonly IUnidadDeTrabajo _uow;
        private readonly IMapper _mapper;
        private readonly IActualizarRecursosLogica _actualizarRecursosLogica;
        

        public EventoLogica(IEventoRepositorio eventoRepositorio, IPartidaRepositorio partidaRepositorio, IUnidadDeTrabajo uow, 
            IMapper mapper, IActualizarRecursosLogica actualizarRecursosLogica)
        {
            _eventoRepositorio = eventoRepositorio;
            _partidaRepositorio = partidaRepositorio;
            _uow = uow;
            _mapper = mapper;
            _actualizarRecursosLogica = actualizarRecursosLogica;
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

            // Aplicar efectos según decisión (ahora con CU global para actualizar los recursos)
            _actualizarRecursosLogica.ActualizarRecursosAsync(
                partida,
                (acepto ? evento.FelicidadAceptar : evento.FelicidadRechazar),
                (acepto ? evento.ContaminacionAceptar : evento.ContaminacionRechazar),
                (acepto ? evento.EcoCoinsAceptar : 0),
                cambioEnergia: 0
            );

            evento.Resuelto = true;

            await _eventoRepositorio.Actualizar(evento);

            await _partidaRepositorio.Actualizar(partida);

            await this._uow.CommitAsync();

            var resultadoDTO = _mapper.Map<EventoResueltoDTO>(
                evento,
                opt => opt.Items.Add("Aceptado", acepto)
            );

            return resultadoDTO;
        }
    }
}
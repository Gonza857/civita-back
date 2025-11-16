using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Logica;
using CivitaBack.Logica.Hubs;
using CivitaBack.Logica.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    public class EventoController : BaseApiController
    {
        private readonly IEventoLogica _eventoLogica;
        private readonly IHubContext<EventoHub> _hubContext;

        public EventoController(
            IEventoLogica eventoLogica, 
            IHubContext<EventoHub> hubContext,
            IMapper mapper
            ) : base(mapper)
        {
            _eventoLogica = eventoLogica;
            _hubContext = hubContext;
        }

        [HttpPost("disparar/{partidaId}")]
        public async Task<IActionResult> DispararEvento(int partidaId)
        {
            try
            {
                var eventoDisparado = await _eventoLogica.DispararEventoAsync(partidaId);

                await _hubContext.Clients.Group(partidaId.ToString())
                .SendAsync("EventoDisparado", eventoDisparado);

                return Ok(eventoDisparado);
            }
            catch (AccesoDenegadoExcepcion ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió un error al disparar el evento.");
            }
        }

        [HttpPost("resolver/{eventoId}")]
        public async Task<IActionResult> ResolverEvento(int eventoId, [FromQuery] string respuesta)
        {

            try
            {
                var resultado = await _eventoLogica.ResolverEventoPreguntaAsync(eventoId, respuesta);

                await _hubContext.Clients.Group(resultado.PartidaId.ToString())
                .SendAsync("EventoResuelto", resultado);

                return Ok(resultado);
            }
            catch (AccesoDenegadoExcepcion ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió un error al resolver el evento.");
            }
        }
    }
}

using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Logica;
using CivitaBack.Logica.Hubs;
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

                return Ok(base.Mapear<EventoDisparadoDTO>(eventoDisparado));
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió un error al disparar el evento.");
            }
        }

        [HttpPost("resolver/{eventoId}")]
        public async Task<IActionResult> ResolverEvento(int eventoId, [FromQuery] bool acepto)
        {

            try
            {
                var resultado = await _eventoLogica.ResolverEventoAsync(eventoId, acepto);

                await _hubContext.Clients.Group(resultado.PartidaId.ToString())
                .SendAsync("EventoResuelto", resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió un error al resolver el evento.");
            }
        }
    }
}

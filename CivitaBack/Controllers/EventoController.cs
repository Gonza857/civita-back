using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEventoLogica _eventoLogica;
        public EventoController(IEventoLogica eventoLogica)
        {
            _eventoLogica = eventoLogica;
        }

        [HttpPost("disparar/{partidaId}")]
        public async Task<IActionResult> DispararEvento(int partidaId)
        {
            try
            {
                var eventoDisparado = await _eventoLogica.DispararEventoAsync(partidaId);
                return Ok(eventoDisparado);
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

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió un error al resolver el evento.");
            }
        }
    }
}

using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursosController : ControllerBase
    {

        private readonly IRecursoLogica _recursoLogica;
        public RecursosController(IRecursoLogica rl)
        {
            this._recursoLogica = rl;
        }

        [HttpGet("{idPartida}")]
        public IActionResult GetRecursosPartida(int idPartida)
        {
            try
            {
                RecursoDTO recursos = this._recursoLogica.ObtenerRecursos(idPartida);
                return Ok(recursos);
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió un error");
            }
        }

        //  Aumentar energía
        [HttpPost("subirEnergia/{idPartida}")]
        public IActionResult SubirEnergia(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarEnergia(idPartida, 10);
                return Ok(new { mensaje = "Energía aumentada correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        //  Reducir energía
        [HttpPost("bajarEnergia/{idPartida}")]
        public IActionResult BajarEnergia(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarEnergia(idPartida, -10);
                return Ok(new { mensaje = "Energía reducida correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }
    }
}

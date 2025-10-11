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


    }
}

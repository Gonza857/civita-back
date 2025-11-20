using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    public class RecursosController : BaseApiController
    {

        private readonly IRecursoLogica _recursoLogica;
        private readonly ILogger<RecursosController> _logger;
        public RecursosController(
            IRecursoLogica rl, 
            ILogger<RecursosController> logger, 
            IMapper mapper) : base (mapper)
        {
            this._recursoLogica = rl;
            this._logger = logger;
        }

        [HttpGet("{idPartida}")]
        public async Task<IActionResult> GetRecursosPartida(int idPartida)
        {
            try
            {
                var recurso = await this._recursoLogica.ObtenerRecursos(idPartida);
                return Ok(base.Mapear<RecursoDTO>(recurso));
            }
            catch (AccesoDenegadoExcepcion ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error");
            }
        }
        
        [HttpPost("subirEnergia/{idPartida}")]
        public async Task<IActionResult> SubirEnergia(int idPartida)
        {
            try
            {
                await _recursoLogica.ModificarEnergia(idPartida, 10);
                return Ok(new { mensaje = "Energía aumentada correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        //  Reducir energía
        [HttpPost("bajarEnergia/{idPartida}")]
        public async Task<IActionResult> BajarEnergia(int idPartida)
        {
            try
            {
                await _recursoLogica.ModificarEnergia(idPartida, -10);
                return Ok(new { mensaje = "Energía reducida correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }
    }
}

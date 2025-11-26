using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Hangfire.PostgreSql.Properties;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    public class RecursosController : BaseApiController
    {

        private readonly IRecursoLogica _recursoLogica;
        private readonly ILogger<RecursosController> _logger;
        private readonly IActualizarRecursosLogica _crudRecursosLogica;
        private readonly IPartidaLogica _partidaLogica;
        public RecursosController(
            IRecursoLogica rl,
            ILogger<RecursosController> logger,
            IActualizarRecursosLogica rcl,
            IPartidaLogica pl, 
            IMapper mapper) : base(mapper)

        {
            this._recursoLogica = rl;
            this._logger = logger;
            this._crudRecursosLogica = rcl;
            this._partidaLogica = pl;
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


        [HttpPost("ImpactarResultado")]
        public async Task<IActionResult> ResultadoMinijuego(MinijuegosDTO resultados)
        {
            try
            {
                var partidaActual = await _partidaLogica.ObtenerPorId(resultados.PartidaId);
                var recursoAdevolver =  await _recursoLogica.ImpactarPremiosMiniJuego(partidaActual, resultados.Recurso);
                var devolver = base.Mapear <RecursoDTO>(recursoAdevolver);
                return Ok(new { exito = true, mensaje = "Recursos actualizados correctamente!" ,devolver} ) ;
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error al impactar los resultados.");
            }

        }

    }
}

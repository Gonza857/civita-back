using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica.Backgrounds;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CicloController : ControllerBase
    {
        private readonly ICicloLogica _cicloLogica;
        private readonly ILogger<CicloController> _logger;
        private readonly BackgroundCicloLogica _cicloBackground;
        private readonly IPartidaLogica _partidaLogica;

        public CicloController(ICicloLogica cicloLogica, ILogger<CicloController> logger, BackgroundCicloLogica background, IPartidaLogica partidaLogica)
        {
            _cicloLogica = cicloLogica;
            _logger = logger;
            _logger.LogInformation("CicloController instanciado");
            _cicloBackground = background;
            _partidaLogica = partidaLogica;
        }

        /// <summary>
        /// Ejecuta manualmente el procesamiento de ciclos para todas las partidas.
        /// </summary>
        [HttpPost("procesar")]
        public async Task<IActionResult> ProcesarCicloManual()
        {
            try
            {
                _logger.LogInformation("⚙️ Procesando ciclo manualmente a las {Hora}", DateTime.Now);
                _logger.LogInformation("Entrando al endpoint ProcesarCicloManual");

                await _cicloLogica.EjecutarCicloAsync();

                _logger.LogInformation("Terminó EjecutarCicloAsync");

                _logger.LogInformation("✅ Ciclo manual procesado correctamente a las {Hora}", DateTime.Now);
                return Ok(new { mensaje = "Ciclo procesado correctamente", fecha = DateTime.Now });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al procesar el ciclo manual");
                return StatusCode(500, new { mensaje = "Error al procesar el ciclo", error = ex.Message });
            }
        }

        [HttpPost("pausar/{idPartida}")]
        public async Task<IActionResult> PausarCiclo(int idPartida)
        {
            try
            {
                var partida = await _partidaLogica.ObtenerPorId(idPartida);
                if (partida == null)
                    return NotFound(new { mensaje = "Partida no encontrada" });

                await _cicloLogica.PausarCiclo(partida);

                return Ok(new { mensaje = "Ciclo pausado" });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al pausar el ciclo");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPost("continuar/{idPartida}")]
        public async Task<IActionResult> ContinuarCiclo(int idPartida)
        {
            try
            {
                var partida = await _partidaLogica.ObtenerPorId(idPartida);
                if (partida == null)
                    return NotFound(new { mensaje = "Partida no encontrada" });

                await _cicloLogica.ContinuarCiclo(partida);

                return Ok(new { mensaje = "Ciclo continuado" });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al continuar el ciclo");
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

    }
}

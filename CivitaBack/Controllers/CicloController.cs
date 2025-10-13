using CivitaBack.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CicloController : ControllerBase
    {
        private readonly ICicloLogica _cicloLogica;
        private readonly ILogger<CicloController> _logger;

        public CicloController(ICicloLogica cicloLogica, ILogger<CicloController> logger)
        {
            _cicloLogica = cicloLogica;
            _logger = logger;
            _logger.LogInformation("CicloController instanciado");

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

    }
}

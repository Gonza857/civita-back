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

        public CicloController(ICicloLogica cicloLogica, ILogger<CicloController> logger, BackgroundCicloLogica background)
        {
            _cicloLogica = cicloLogica;
            _logger = logger;
            _logger.LogInformation("CicloController instanciado");
            _cicloBackground = background; 

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

        [HttpPost("pausar")]
        public IActionResult Pausar()
        {
            _cicloBackground.Pausar();
            return Ok(new { mensaje = "Ciclo pausado" });
        }

        [HttpPost("continuar")]
        public IActionResult Continuar()
        {
            _cicloBackground.Continuar();
            return Ok(new { mensaje = "Ciclo continuado" });
        }

    }
}

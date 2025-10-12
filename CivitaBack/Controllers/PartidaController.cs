using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;
using CivitaBack.Data.DTO;
using System.Threading.Tasks;

namespace CivitaBack.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly IPartidaLogica _partidaServicio;

        public PartidaController(IPartidaLogica partidaLogica)
        {
            _partidaServicio = partidaLogica;
        }

        // 🧱 Crear nueva partida
        [HttpPost]
        public IActionResult PostPartida()
        {
            var partida = _partidaServicio.CrearPartida();
            return Ok(partida);
        }

        // 📜 Obtener todas las partidas
        [HttpGet]
        public IActionResult GetPartidas()
        {
            var partidas = _partidaServicio.ObtenerPartidas();
            return Ok(partidas);
        }

        // 💾 Guardar mapa (JSON + estructuras)
        [HttpPost("guardar-mapa")]
        public async Task<IActionResult> GuardarMapa([FromBody] GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId <= 0)
                return BadRequest("Datos de mapa inválidos.");

            try
            {
                await _partidaServicio.GuardarMapaAsync(dto);
                return Ok(new { mensaje = "Mapa guardado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al guardar el mapa.", detalle = ex.Message });
            }
        }

        // 🧩 Obtener mapa (objeto completo con estructuras y json)
        [HttpGet("{partidaId}/mapa")]
        public async Task<IActionResult> ObtenerMapa(int partidaId)
        {
            try
            {
                var partida = await _partidaServicio.ObtenerMapaAsync(partidaId);
                if (partida == null)
                    return NotFound(new { mensaje = "No se encontró la partida solicitada." });

                return Ok(new
                {
                    partida.Id,
                    partida.UsuarioId,
                    partida.UltimaVez,
                    json = partida.JsonMapa,
                    estructuras = partida.EstructuraEnMapa?.Select(e => new
                    {
                        e.Id,
                        e.EstructuraId,
                        e.X,
                        e.Y,
                        e.Width,
                        e.Height
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener el mapa.", detalle = ex.Message });
            }
        }

        // 🌍 Obtener solo el JSON del mapa (para Phaser)
        [HttpGet("{partidaId}/json")]
        public async Task<IActionResult> ObtenerJsonMapa(int partidaId)
        {
            try
            {
                var partida = await _partidaServicio.ObtenerMapaAsync(partidaId);
                if (partida == null)
                    return NotFound(new { mensaje = "No se encontró la partida." });

                if (string.IsNullOrWhiteSpace(partida.JsonMapa))
                    return Ok(new { mensaje = "La partida no tiene un mapa guardado aún.", json = "{}" });

                // Devuelve el JSON directamente con el Content-Type correcto
                return Content(partida.JsonMapa, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener el JSON del mapa.", detalle = ex.Message });
            }
        }

        // 🔄 Actualizar mapa (reemplaza JSON y estructuras existentes)
        [HttpPut("{partidaId}/actualizar-mapa")]
        public async Task<IActionResult> ActualizarMapa(int partidaId, [FromBody] GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId != partidaId)
                return BadRequest(new { mensaje = "Datos inválidos o ID de partida no coincide." });

            try
            {
                await _partidaServicio.ActualizarMapaAsync(dto);
                return Ok(new { mensaje = "Mapa actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el mapa.", detalle = ex.Message });
            }
        }
    }
}

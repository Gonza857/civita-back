using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidaController : ControllerBase
    {
        private readonly IPartidaLogica _partidaLogica;
        private readonly IRecursoLogica _recursoLogica;
        private readonly IAuthLogica _authLogica;
        private readonly IUsuarioLogica _usuarioLogica;
        private readonly IEstructuraLogica _estructuraLogica;
        private readonly IEstructuraMapaLogica _estructuraMapaLogica;

        public PartidaController(
            IPartidaLogica partidaLogica,
            IRecursoLogica recursoLogica,
            IAuthLogica authLogica,
            IUsuarioLogica usuarioLogica,
            IEstructuraLogica el,
            IEstructuraMapaLogica estructuraMapaLogica)
        {
            _partidaLogica = partidaLogica;
            _recursoLogica = recursoLogica;
            _authLogica = authLogica;
            _usuarioLogica = usuarioLogica;
            _estructuraLogica = el;
            _estructuraMapaLogica = estructuraMapaLogica;
        }

        // 🧱 Crear partida inicial y configurar recursos
        [HttpPost("Iniciar/{idUsuario}")]
        public IActionResult Iniciar(int idUsuario)
        {
            try
            {
                var partida = _partidaLogica.CrearPartida(idUsuario);
                _recursoLogica.ConfigurarInicial(partida);
                return Ok(partida);
            }
            catch (ErrorInternoExcepction)
            {
                return Problem("Ocurrió un error al guardar la partida.");
            }
        }

        // Obtener la partida de un usuario
        [HttpGet("porUsuario/{idUsuario}")]
        public IActionResult ObtenerPartidaPorUsuario(int idUsuario)
        {
            try
            {
                var partida = _partidaLogica.ObtenerPorUsuarioId(idUsuario);

                if (partida == null)
                    return NotFound(new { error = "No se encontró la partida para el usuario especificado." });

                return Ok(partida);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Error al obtener la partida: {ex.Message}" });
            }
        }

        // 📜 Obtener todas las partidas
        [HttpGet]
        public IActionResult GetPartidas()
        {
            var partidas = _partidaLogica.ObtenerPartidas();
            return Ok(partidas);
        }

        // ✏️ Actualizar datos de una partida (no mapa)
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPartida([FromBody] PartidaDTO partidaDTO, int id)
        {
            try
            {
                var usuario = await _usuarioLogica.ObtenerPorId(partidaDTO.UsuarioId);
                _partidaLogica.Actualizar(partidaDTO, usuario);
                return Ok();
            }
            catch (PartidaExcepcion ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error en el servidor.");
            }
        }

        // 🔎 Obtener partida por ID de usuario
        [HttpGet("{id}")]
        public IActionResult GetPartidaPorId(int id)
        {
            var partida = _partidaLogica.ObtenerPorUsuarioId(id);
            return Ok(partida);
        }

        // 💾 Guardar mapa (JSON + estructuras)
        [HttpPost("guardar-mapa")]
        public async Task<IActionResult> GuardarMapa([FromBody] GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId <= 0)
                return BadRequest("Datos de mapa inválidos.");

            try
            {
                await _partidaLogica.GuardarMapaAsync(dto);
                return Ok(new { mensaje = "Mapa guardado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al guardar el mapa.", detalle = ex.Message });
            }
        }

        // 🧩 Obtener mapa completo (con estructuras)
        [HttpGet("{partidaId}/mapa")]
        public async Task<IActionResult> ObtenerMapa(int partidaId)
        {
            try
            {
                var partida = await _partidaLogica.ObtenerMapaAsync(partidaId);
                if (partida == null)
                    return NotFound(new { mensaje = "No se encontró la partida solicitada." });

                return Ok(new
                {
                    partida.Id,
                    partida.UsuarioId,
                    partida.UltimaVez,
                    json = partida.JsonMapa,
                    estructuras = partida.EstructuraMapa?.Select(e => new
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
                var partida = await _partidaLogica.ObtenerMapaAsync(partidaId);
                if (partida == null)
                    return NotFound(new { mensaje = "No se encontró la partida." });

                if (string.IsNullOrWhiteSpace(partida.JsonMapa))
                    return Ok(new { mensaje = "La partida no tiene un mapa guardado aún.", json = "{}" });

                return Content(partida.JsonMapa, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener el JSON del mapa.", detalle = ex.Message });
            }
        }

        // 🔄 Actualizar mapa existente (JSON + estructuras)
        [HttpPut("{partidaId}/actualizar-mapa")]
        public async Task<IActionResult> ActualizarMapa(int partidaId, [FromBody] GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId != partidaId)
                return BadRequest(new { mensaje = "Datos inválidos o ID de partida no coincide." });

            try
            {
                await _partidaLogica.ActualizarMapaAsync(dto);
                return Ok(new { mensaje = "Mapa actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el mapa.", detalle = ex.Message });
            }
        }

        // [HttpPost("Colocar/{partidaId}/{estructuraId}/usuario/{idUsuario}")]
        // public async Task<IActionResult> ColocarEstructura(int partidaId, int estructuraId, int idUsuario)
        // {
        //     try
        //     {
        //         Partida partida = _partidaLogica.ObtenerCompletaPorUsuarioId(idUsuario);
        //         Estructura estructura = _estructuraLogica.ObtenerPorId(estructuraId);
        //         _estructuraMapaLogica.Colocar(estructura, partida);
        //         return Ok();
        //     } catch (Exception ex)
        //     {
        //         return Problem(ex.Message);
        //     }
        //
        // }
    }
}

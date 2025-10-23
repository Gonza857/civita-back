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
        private readonly ILogroPartidaLogica _logroPartidaLogica;
        private readonly ILogger<PartidaController> _logger;

        public PartidaController(
            IPartidaLogica partidaLogica,
            IRecursoLogica recursoLogica,
            IAuthLogica authLogica,
            IUsuarioLogica usuarioLogica,
            IEstructuraLogica el,
            IEstructuraMapaLogica estructuraMapaLogica,
            ILogger<PartidaController> logger,
            ILogroPartidaLogica logroPartidaLogica)
        {
            _partidaLogica = partidaLogica;
            _recursoLogica = recursoLogica;
            _authLogica = authLogica;
            _usuarioLogica = usuarioLogica;
            _estructuraLogica = el;
            _estructuraMapaLogica = estructuraMapaLogica;
            _logroPartidaLogica = logroPartidaLogica;
            _logger = logger;
        }

        //PARA EXPO
        private const int USUARIO_EXPO = 9999;

        [HttpPost("expo/iniciar")]
        public async Task<IActionResult> IniciarDemo()
        {
            try
            {
                var partida = await _partidaLogica.CrearPartida(USUARIO_EXPO);
                await _recursoLogica.ConfigurarInicial(partida);
                return Ok(partida);
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ErrorInternoExcepction ex)
            {
                return Problem("Ocurrió un error al guardar la partida.");
            }
        }

        [HttpPost("expo/reiniciar")]
        public async Task<IActionResult> ReiniciarDemo()
        {
            var partida = await _partidaLogica.ObtenerPorUsuarioId(12);

            if (partida == null)
                return NotFound("No hay partida Demo para reiniciar");

            await _recursoLogica.ConfigurarInicial(partida);
            await _estructuraMapaLogica.ReiniciarEstructurasDePartida(partida.Id);
            await _logroPartidaLogica.ReiniciarLogros(partida.Id);

            return Ok("Partida Demo reiniciada");
        }

        // 🧱 Crear partida inicial y configurar recursos
        [HttpPost("Iniciar/{idUsuario}")]
        public async Task<IActionResult> Iniciar(int idUsuario)
        {
            try
            {
                var partida = await _partidaLogica.CrearPartida(idUsuario);
                await _recursoLogica.ConfigurarInicial(partida);
                //Ver estructuras iniciales segun mapa
                return Ok(partida);
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ErrorInternoExcepction ex)
            {
                return Problem("Ocurrió un error al guardar la partida.");
            }
        }

        // Obtener la partida de un usuario
        [HttpGet("porUsuario/{idUsuario}")] // -> PascalCase -> PorUsuario/{idUsuario}
        public async Task<IActionResult> ObtenerPartidaPorUsuario(int idUsuario)
        {
            try
            {
                Partida partida = await _partidaLogica.ObtenerPorUsuarioId(12);
                return Ok(partida);
            }
            catch (PartidaExcepcion ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Error al obtener la partida");
            }
        }

        // 📜 Obtener todas las partidas
        [HttpGet]
        public async Task<IActionResult> GetPartidas()
        {
            try
            {
                var partidas = await _partidaLogica.ObtenerPartidas();
                return Ok(partidas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error al obtener las partidas");
            }
        }

        // ✏️ Actualizar datos de una partida (no mapa)
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPartida([FromBody] PartidaDTO? partidaDTO, int id)
        {
            if (partidaDTO == null)
                return BadRequest("No se encontró la partida.");
            
            try
            {
                var usuario = await _usuarioLogica.ObtenerPorId(partidaDTO.UsuarioId);
                await _partidaLogica.Actualizar(partidaDTO, usuario);
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
        public async Task<IActionResult> GetPartidaPorId(int id)
        {
            try
            {
                var partida = await _partidaLogica.ObtenerPorUsuarioId(id);
                return Ok(partida);
            }
            catch (PartidaExcepcion ex)
            {
                return NotFound("No se encontró la partida");
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió unerror al obtener la partida");
            }
  
        }

        // 💾 Guardar mapa (JSON + estructuras)
        [HttpPost("guardar-mapa")]
        public async Task<IActionResult> GuardarMapa([FromBody] GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId <= 0)
                return BadRequest("Datos de mapa inválidos.");

            try
            {
                await _partidaLogica.ActualizarMapaDePartidaAsync(dto);
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
        public async Task<IActionResult> ActualizarMapa(int partidaId, [FromBody] GuardarMapaDTO? dto)
        {
            if (dto == null || dto.PartidaId != partidaId)
                return BadRequest("Datos inválidos o ID de partida no coincide.");

            try
            {
                await _partidaLogica.ActualizarMapaDePartidaAsync(dto);
                return Ok(new { mensaje = "Mapa actualizado correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest("Error al actualizar el mapa: " + ex.Message);
            }
            catch (Exception ex)
            {
                return Problem("Error al actualizar el mapa.");
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

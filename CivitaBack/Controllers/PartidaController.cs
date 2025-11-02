using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class PartidaController : BaseApiController
{
    private readonly IPartidaLogica _partidaLogica;
    private readonly IRecursoLogica _recursoLogica;
    private readonly IUsuarioLogica _usuarioLogica;
    private readonly IEstructuraMapaLogica _estructuraMapaLogica;
    private readonly ILogroPartidaLogica _logroPartidaLogica;
    private readonly ILogger<PartidaController> _logger;
    
    public PartidaController(
        IPartidaLogica partidaLogica,
        IRecursoLogica recursoLogica,
        IUsuarioLogica usuarioLogica,
        IEstructuraMapaLogica estructuraMapaLogica,
        ILogger<PartidaController> logger,
        ILogroPartidaLogica logroPartidaLogica,
        IMapper mapper) : base(mapper)
    {
        _partidaLogica = partidaLogica;
        _recursoLogica = recursoLogica;
        _usuarioLogica = usuarioLogica;
        _estructuraMapaLogica = estructuraMapaLogica;
        _logroPartidaLogica = logroPartidaLogica;
        _logger = logger;
    }

    [HttpPost("expo/iniciar")]
    public async Task<IActionResult> IniciarDemo()
    {
        try
        {
            var partida = await _partidaLogica.CrearPartida(1);
            await _recursoLogica.ConfigurarInicial(partida);
            return Ok(base.Mapear<PartidaDTO>(partida));
        }
        catch (PartidaExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ErrorInternoExcepction ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al guardar la partida.");
        }
    }

    [HttpPost("expo/reiniciar")]
    public async Task<IActionResult> ReiniciarDemo()
    {
        var partida = await _partidaLogica.ObtenerPorUsuarioId(1);

        if (partida == null)
            return NotFound("No hay partida Demo para reiniciar");

        await _recursoLogica.ConfigurarInicial(partida);
        await _estructuraMapaLogica.ReiniciarEstructurasDePartida(partida.Id);
        await _logroPartidaLogica.ReiniciarLogros(partida.Id);

        return Ok("Partida Demo reiniciada");
    }

    [HttpPost("Iniciar/{idUsuario}")]
    public async Task<IActionResult> Iniciar(int idUsuario)
    {
        try
        {
            var partida = await _partidaLogica.CrearPartida(idUsuario);
            await _recursoLogica.ConfigurarInicial(partida);
            //Ver estructuras iniciales segun mapa
            return Ok(base.Mapear<PartidaDTO>(partida));
        }
        catch (PartidaExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ErrorInternoExcepction ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al guardar la partida.");
        }
    }

    // Obtener la partida de un usuario
    [HttpGet("porUsuario/{idUsuario}")] // -> PascalCase -> PorUsuario/{idUsuario}
    public async Task<IActionResult> ObtenerPartidaPorUsuario(int idUsuario)
    {
        try
        {
            idUsuario = 1;
            Partida partida = await _partidaLogica.ObtenerPorUsuarioId(idUsuario);
            return Ok(base.Mapear<PartidaDTO>(partida));
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
            return Ok(base.MapearLista<PartidaDTO>(partidas));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener las partidas");
        }
    }

    // ✏️ Actualizar datos de una partida (no mapa)
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchPartida([FromBody] PartidaDTO? partidaDto, int id)
    {
        if (partidaDto == null)
            return BadRequest("No se encontró la partida.");

        try
        {
            var usuario = await _usuarioLogica.ObtenerPorId(partidaDto.UsuarioId);
            await _partidaLogica.Actualizar(base.Mapear<Partida>(partidaDto), usuario);
            return Ok();
        }
        catch (PartidaExcepcion ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            return Ok(base.Mapear<PartidaDTO>(partida));
        }
        catch (PartidaExcepcion ex)
        {
            return NotFound("No se encontró la partida");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió unerror al obtener la partida");
        }
    }

    // 💾 Guardar mapa (JSON + estructuras)
    [HttpPost("guardar-mapa")]
    public async Task<IActionResult> GuardarMapa([FromBody] GuardarMapaDTO dto)
    {
        if (dto == null || dto.PartidaId <= 0 || dto.Estructuras == null || dto.Estructuras.Count <= 0)
            return BadRequest("Datos de mapa inválidos.");

        try
        {
            var estructuras = base.MapearLista<EstructuraMapa>(dto.Estructuras);
            await _partidaLogica.ActualizarMapaDePartidaAsync(dto.PartidaId, dto.JsonMapa, estructuras);
            return Ok(new { mensaje = "Mapa guardado correctamente." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Error al guardar el mapa.");
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
                return NotFound("No se encontró la partida.");
            return Ok(base.Mapear<PartidaMapaDTO>(partida));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Error al obtener la partida y el mapa.");
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
                return NotFound("No se encontró la partida.");

            if (string.IsNullOrWhiteSpace(partida.JsonMapa))
                return NotFound("Mapa no encontrado.");

            return Ok(partida.JsonMapa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Error al obtener el mapa.");
        }
    }

    // 🔄 Actualizar mapa existente (JSON + estructuras)
    [HttpPut("{partidaId}/actualizar-mapa")]
    public async Task<IActionResult> ActualizarMapa(int partidaId, [FromBody] GuardarMapaDTO? dto)
    {
        if (dto == null || dto.PartidaId <= 0 || dto.Estructuras == null || dto.Estructuras.Count <= 0)
            return BadRequest("Datos inválidos o ID de partida no coincide.");

        try
        {
            var estructuras = base.MapearLista<EstructuraMapa>(dto.Estructuras);
            await _partidaLogica.ActualizarMapaDePartidaAsync(dto.PartidaId, dto.JsonMapa, estructuras);
            return Ok(new { mensaje = "Mapa actualizado correctamente." });
        }
        catch (PartidaExcepcion ex)
        {
            return BadRequest("Error al actualizar el mapa: " + ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Error al actualizar el mapa.");
        }
    }

        [HttpDelete("expo/eliminar-estructura")]
        public async Task<IActionResult> EliminarEstructura([FromBody] EliminarEstructuraDTO dto)
        {
            try
            {
                await _estructuraMapaLogica.EliminarEstructuraAsync(base.Mapear<EstructuraMapa>(dto));
                return Ok("Estructura eliminada correctamente");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Error eliminando la estructura");
            }
        }
}

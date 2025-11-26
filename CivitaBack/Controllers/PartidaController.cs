using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class PartidaController : BaseApiController
{
    private readonly IPartidaLogica _partidaLogica;
    private readonly IRecursoLogica _recursoLogica;
    private readonly IUsuarioLogica _usuarioLogica;
    private readonly IMisionLogica _misionLogica;
    private readonly IEstructuraMapaLogica _estructuraMapaLogica;
    private readonly ILogroPartidaLogica _logroPartidaLogica;
    private readonly IMapaLogica _mapaLogica;
    private readonly ICompraEstructurasLogica _compraEstructurasLogica;
    private readonly INivelLogica _nivelLogica;
    private readonly ILogger<PartidaController> _logger;
    private readonly IAccesoUsuarios _accesoUsuarios;


    public PartidaController(
        IPartidaLogica partidaLogica,
        IRecursoLogica recursoLogica,
        IUsuarioLogica usuarioLogica,
        IMisionLogica misionLogica,
        IEstructuraMapaLogica estructuraMapaLogica,
        ILogger<PartidaController> logger,
        ILogroPartidaLogica logroPartidaLogica,
        IMapaLogica mapaLogica,
        ICompraEstructurasLogica compraEstructurasLogica,
        INivelLogica nivelLogica,
        IAccesoUsuarios accesoUsuarios,
        IMapper mapper) : base(mapper)
    {
        _partidaLogica = partidaLogica;
        _recursoLogica = recursoLogica;
        _usuarioLogica = usuarioLogica;
        _misionLogica = misionLogica;
        _estructuraMapaLogica = estructuraMapaLogica;
        _logroPartidaLogica = logroPartidaLogica;
        _mapaLogica = mapaLogica;
        _compraEstructurasLogica = compraEstructurasLogica;
        _logger = logger;
        _nivelLogica = nivelLogica;
        _accesoUsuarios = accesoUsuarios;
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
        catch (ErrorInternoException ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al guardar la partida.");
        }
    }

    [HttpPost("Reiniciar/{id}")]
    public async Task<IActionResult> ReiniciarDemo(int id)
    {
        try
        {
            var partida = await _partidaLogica.ObtenerPorId(id);
            await _recursoLogica.ConfigurarInicial(partida);
            await this._misionLogica.ResetMisiones(TipoMision.Diaria);
            await _estructuraMapaLogica.ReiniciarEstructurasDePartida(partida.Id);
            await _logroPartidaLogica.ReiniciarLogros(partida.Id);
            return Ok("Partida Demo reiniciada");
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al resetear.");
        }

    }

    [HttpPost("Iniciar/{idUsuario}")]
    public async Task<IActionResult> Iniciar(int idUsuario)
    {
        try
        {
            var partida = await _partidaLogica.CrearPartida(idUsuario);
            await _recursoLogica.ConfigurarInicial(partida);
            return Ok(base.Mapear<PartidaDTO>(partida));
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (PartidaExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ErrorInternoException ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al guardar la partida.");
        }
    }

    // Obtener la partida de un usuario
    
    [HttpGet("porUsuario/{idUsuario}")] // -> PascalCase -> PorUsuario/{idUsuario}
    [Authorize(Roles = "Jugador, Desconocido, Admin")]
    public async Task<IActionResult> ObtenerPartidaPorUsuario(int idUsuario)
    {
        try
        {
            _accesoUsuarios.ValidarAcceso(idUsuario);
            
            Partida partida = await _partidaLogica.ObtenerPorUsuarioId(idUsuario);
            await this._nivelLogica.VerificarNivel(partida);
            
            int xpSiguienteNivel =  this._nivelLogica.ObtenerExperienciaTechoNivel(partida.Nivel);
            
            var dto = base.Mapear<PartidaDTO>(partida);
            dto.ExperienciaSiguienteNivel = xpSiguienteNivel;
            
            return Ok(dto);
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
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
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPartidas()
    {
        try
        {
            var partidas = await _partidaLogica.ObtenerPartidas();
            return Ok(base.MapearLista<PartidaDTO>(partidas));
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener las partidas");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchPartida([FromBody] PartidaDTO? partidaDto, int id)
    {
        
        try
        {
            var usuario = await _usuarioLogica.ObtenerPorId(partidaDto.UsuarioId);
            await _partidaLogica.Actualizar(base.Mapear<Partida>(partidaDto), usuario);
            return Ok();
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
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

    // 🔎 Obtener partida por ID 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPartidaPorId(int id)
    {
        try
        {
            var partida = await _partidaLogica.ObtenerPorId(id);
            return Ok(base.Mapear<PartidaDTO>(partida));
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
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
        if (dto.PartidaId <= 0 || dto.Estructuras == null || dto.Estructuras.Count <= 0)
            return BadRequest("Datos de mapa inválidos.");

        try
        {
            var estructuras = base.MapearLista<EstructuraMapa>(dto.Estructuras);
            await _mapaLogica.ActualizarMapaDePartidaAsync(dto.PartidaId, dto.JsonMapa, estructuras);
            return Ok(new { mensaje = "Mapa guardado correctamente." });
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
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
            var partida = await _mapaLogica.ObtenerMapaAsync(partidaId);
            if (partida == null)
                return NotFound("No se encontró la partida.");
            return Ok(base.Mapear<PartidaMapaDTO>(partida));
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
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
            var partida = await _mapaLogica.ObtenerMapaAsync(partidaId);
            if (partida == null)
                return NotFound("No se encontró la partida.");

            if (string.IsNullOrWhiteSpace(partida.JsonMapa))
                return NotFound("Mapa no encontrado.");

            return Ok(partida.JsonMapa);
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
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
            await _mapaLogica.ActualizarMapaDePartidaAsync(dto.PartidaId, dto.JsonMapa, estructuras);
            return Ok(new { mensaje = "Mapa actualizado correctamente." });
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
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
            var estructurasEliminar = base.Mapear<EstructuraMapa>(dto);
            await _estructuraMapaLogica.EliminarEstructuraAsync(estructurasEliminar);
            return Ok("Estructura eliminada correctamente");
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (EstructuraMapaException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Error eliminando la estructura");
        }
    }

    [HttpPost("comprar-estructura/{partidaId}/{estructuraId}")]
    public async Task<IActionResult> ComprarEstructura(int partidaId, int estructuraId)
    {
        try
        {
            int nuevoSaldo = await _compraEstructurasLogica.ComprarEstructuraAsync(partidaId, estructuraId);

            return Ok(new { nuevoSaldo = nuevoSaldo });
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (PartidaExcepcion ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return Problem("Ocurrió un error interno al procesar la compra.");
        }
    }

    [HttpGet("PuedeSubirNivel/{idPartida}")]
    public async Task<IActionResult> SaberSiPuedeSubirNivel(int idPartida)
    {
        try
        {
            Partida p = await _partidaLogica.ObtenerPorId(idPartida);
            bool puede = this._nivelLogica.PuedeSubir(p.Experiencia, p.Nivel);
            return Ok(puede);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error.");
        }
    }
    
    [HttpGet("ExperienciaSiguienteNivel/{idPartida}")]
    public async Task<IActionResult> SaberExperienciaParaSiguienteNivel(int idPartida)
    {
        try
        {
            Partida p = await _partidaLogica.ObtenerPorId(idPartida);
            int cantidad = this._nivelLogica.ObtenerExperienciaTechoNivel(p.Nivel);
            return Ok(cantidad);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error.");
        }
    }
    
    [HttpGet("SubirNivelSiEsPosible/{idPartida}")]
    public async Task<IActionResult> SubirNivelSiEsPosible(int idPartida)
    {
        try
        {
            Partida p = await _partidaLogica.ObtenerPorId(idPartida);
            this._nivelLogica.SubirNivel(p);
            await this._partidaLogica.Actualizar(p);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error.");
        }
    }

}

using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using Microsoft.AspNetCore.Mvc;
using LogroExcepcion = CivitaBack.Logica.Excepciones.LogroExcepcion;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class MisionController : BaseApiController
{

    private readonly IMisionLogica _misionLogica;
    private readonly IPartidaLogica _partidaLogica;
    private readonly ILogger<MisionController> _logger;

    public MisionController(
        IMisionLogica iml,
        IMapper mapper,
        IPartidaLogica partidaLogica,
        ILogger<MisionController> logger) : base(mapper)
    {
        _misionLogica = iml;
        this._logger = logger;
        this._partidaLogica = partidaLogica;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var misiones = await _misionLogica.Listado();
            return Ok(base.MapearLista<MisionDTO>(misiones));
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Misiones.");
        }
    }
    
    [HttpGet("Iniciar/{idPartida}")]
    public async Task<IActionResult> EntregarMisiones(int idPartida)
    {
        try
        {
            Partida? partida = await this._partidaLogica.ObtenerPorId(idPartida);
            await _misionLogica.AsignarMisiones(partida);
            return Ok("Misiones asignadas a la partida");
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Misiones.");
        }
    }
    
    [HttpGet("Activas/{idPartida}")]
    public async Task<IActionResult> ObtenerMisionesDisponibles(int idPartida)
    {
        try
        {
            Partida? partida = await this._partidaLogica.ObtenerPorId(idPartida);
            var misiones = await _misionLogica.ObtenerMisionesActivas(partida);
            return Ok(base.MapearLista<MisionDTO>(misiones));
        }
        catch (MisionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Misiones.");
        }
    }
    
    // [HttpGet("DisponibleParaCumplir/{idUsuario}")]
    // public async Task<IActionResult> GetLogrosDisponiblesParaCumplir(int idUsuario)
    // {
    //     try
    //     {
    //         var logros = await _logroLogica.ObtenerListadoInterno();
    //         var partida = await _partidaLogica.ObtenerPartidaPorIdInterno(idUsuario);
    //         var logrosDisponiblesParaCumplir = await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logros);
    //         return Ok(base.MapearLista<LogroDTO>(logrosDisponiblesParaCumplir));
    //     }
    //     catch (LogroExcepcion ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }   
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex.Message);
    //         return Problem("Ocurrió un error al obtener el listado de Logros.");
    //     }
    // }
    //
    // [HttpGet("ReclamarLogros/{idUsuario}")]
    // public async Task<IActionResult> ReclamarLogros(int idUsuario)
    // {
    //     try
    //     {
    //         var logros = await _logroLogica.ObtenerListadoInterno();
    //         var partida = await _partidaLogica.ObtenerPartidaPorIdInterno(idUsuario);
    //         if (partida == null)
    //             return BadRequest("Partida no econtrada");
    //         var logrosFiltrados = await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logros);
    //         await _logroLogica.MarcarLogrosComoCompletados(partida, logrosFiltrados);
    //         await _partidaLogica.ReclamarLogros(partida, logrosFiltrados);
    //         return Ok();
    //     }
    //     catch (LogroExcepcion ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex.Message);
    //         return Problem("Ocurrió un error al obtener el listado de Logros.");
    //     }
    // }
    //
    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] MisionDTO? nuevaMisionDto)
    {
        if (nuevaMisionDto == null)
            return BadRequest("Los datos recibidos son inválidos");
        try
        {
            Mision misionNueva = base.Mapear<Mision>(nuevaMisionDto);
            await _misionLogica.Crear(misionNueva);
            return Ok();
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al guardar el Logro");
        }
    }
    //
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Eliminar(int id)
    // {
    //     try
    //     {
    //         await _logroLogica.Eliminar(id);
    //         return Ok();
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex.Message);
    //         return Problem("Ocurrió un error al eliminar el logro.");
    //     }
    // }
    //
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMisionPorId(int id)
    {
        try
        {
            var mision = await this._misionLogica.ObtenerPorId(id);
            return Ok(base.Mapear<MisionDTO>(mision));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener la mision.");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchMision([FromBody] MisionDTO? misionDto, int id)
    {
        if (misionDto == null)
            return BadRequest("Los datos recibidos son inválidos");

        try
        {
            var mision = base.Mapear<Mision>(misionDto);
            await this._misionLogica.Actualizar(mision, id);
            return Ok();
        }
        catch (MisionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al actualizar la Misión");
        }
    }

}

using CivitaBack.Data.DTO;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LogroController : ControllerBase
{
    private readonly ILogroLogica _logroLogica;
    private readonly IPartidaLogica _partidaLogica;
    public LogroController(ILogroLogica ill, IPartidaLogica ipl)
    {
        this._logroLogica = ill;
        this._partidaLogica = ipl;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var logros = await _logroLogica.ObtenerListado();
            return Ok(logros);
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Problem("Ocurrió un error al obtener el listado de Logros.");
        }
    }
    
    [HttpGet("DisponibleParaCumplir/{idUsuario}")]
    public async Task<IActionResult> GetLogrosDisponiblesParaCumplir(int idUsuario)
    {
        try
        {
            var logros = await _logroLogica.ObtenerListadoInterno();
            var partida = await _partidaLogica.ObtenerPartidaPorIdInterno(idUsuario);
            var logrosDisponiblesParaCumplir = await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logros);
            return Ok(logrosDisponiblesParaCumplir);
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }   
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el listado de Logros.");
        }
    }
    
    [HttpGet("ReclamarLogros/{idUsuario}")]
    public async Task<IActionResult> ReclamarLogros(int idUsuario)
    {
        try
        {
            var logros = await _logroLogica.ObtenerListadoInterno();
            var partida = await _partidaLogica.ObtenerPartidaPorIdInterno(idUsuario);
            if (partida == null)
                return BadRequest("Partida no econtrada");
            var logrosFiltrados = await _logroLogica.ComprobarSiCumpleAlgunLogro(partida, logros);
            await _logroLogica.MarcarLogrosComoCompletados(partida, logrosFiltrados);
            await _partidaLogica.ReclamarLogros(partida, logrosFiltrados);
            return Ok();
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el listado de Logros.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] LogroDTO? nuevoLogro)
    {
        if (nuevoLogro == null)
            return BadRequest("Los datos recibidos son inválidos");
        try
        {
            await _logroLogica.Crear(nuevoLogro);
            return Ok();
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al guardar el Logro");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _logroLogica.Eliminar(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al eliminar el logro.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLogroPorId(int id)
    {
        try
        {
            var logro = await this._logroLogica.ObtenerPorId(id);
            return Ok(logro);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el logro.");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchLogro([FromBody] LogroDTO? logroDTO, int id)
    {
        if (logroDTO == null)
            return BadRequest("Los datos recibidos son inválidos");
        
        try
        {
            await this._logroLogica.Actualizar(logroDTO, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

}

using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogroPartidaController : ControllerBase
{
    private readonly ILogroPartidaLogica _logroPartidaLogica;

    public LogroPartidaController(ILogroPartidaLogica lpl)
    {
        this._logroPartidaLogica = lpl;
    }

    [HttpGet("Completos/{idUsuario}")]
    public async Task<IActionResult> ListadoLogrosCompletados(int idUsuario)
    {
        try
        {
            List<LogroDTO> logros = await this._logroPartidaLogica.ObtenerLogrosCompletados(idUsuario);
            return Ok(logros);
        }
        catch (LogroPartidaExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener los logros completados.");
        }
    }

    [HttpGet("Incompletos/{idUsuario}")]
    public async Task<IActionResult> ListadoLogrosIncompletos(int idUsuario)
    {
        try
        {
            List<LogroDTO> logros = await this._logroPartidaLogica.ObtenerLogrosIncompletos(idUsuario);
            return Ok(logros);
        }
        catch (LogroPartidaExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener los logros no completados.");
        }
    }

}

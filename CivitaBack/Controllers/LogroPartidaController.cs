using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class LogroPartidaController : ControllerBase
{
    private readonly ILogroPartidaLogica _logroPartidaLogica;

    public LogroPartidaController(ILogroPartidaLogica lpl)
    {
        this._logroPartidaLogica = lpl;
    }

    [HttpGet("Completos/{idUsuario}")]
    public IActionResult ListadoLogrosCompletados(int idUsuario)
    {
        try
        {
            List<LogroDTO> logros = this._logroPartidaLogica.ObtenerLogrosCompletados(idUsuario);
            return Ok(logros);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("Incompletos/{idUsuario}")]
    public IActionResult ListadoLogrosIncompletos(int idUsuario)
    {
        try
        {
            List<LogroDTO> logros = this._logroPartidaLogica.ObtenerLogrosIncompletos(idUsuario);
            return Ok(logros);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

}

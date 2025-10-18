using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LogroController : ControllerBase
{

    private readonly ILogroLogica _logroLogica;

    public LogroController(ILogroLogica ill)
    {
        this._logroLogica = ill;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var logros = await _logroLogica.ObtenerListado();
            return Ok(logros);
        }
        catch (Exception)
        {
            return Problem("Ocurrió un error al obtener el listado de Logros.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] LogroDTO? nuevoLogro)
    {
        if (nuevoLogro == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });

        try
        {
            var logroGuardado = await _logroLogica.Guardar(nuevoLogro);
            return Ok(logroGuardado);
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

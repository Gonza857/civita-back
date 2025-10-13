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
    public IActionResult Listado()
    {
        var logros = _logroLogica.ObtenerListado();
        return Ok(logros);
    }

    [HttpPost]
    public IActionResult Guardar([FromBody] LogroDTO nuevoLogro)
    {
        if (nuevoLogro == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });

        var logroGuardado = _logroLogica.Guardar(nuevoLogro);
        return Ok(logroGuardado);
    }

    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        _logroLogica.Eliminar(id);
        return Ok();

    }

    [HttpGet("{id}")]
    public IActionResult GetLogroPorId(int id)
    {
        var logro = this._logroLogica.ObtenerPorId(id);
        return Ok(logro);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchLogro([FromBody] LogroDTO logroDTO, int id)
    {
        try
        {
            this._logroLogica.Actualizar(logroDTO, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }

}

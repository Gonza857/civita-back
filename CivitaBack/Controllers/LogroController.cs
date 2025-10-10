using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;


[ApiController]
[Route("[controller]")]
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
        return Ok(new
        {
            mensaje = "Todo OK",
            exito = true,
            status = 200,
            data = logros
        });
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
        return Ok(new
        {
            mensaje = "Todo OK",
            exito = true,
            status = 200,
            data = 1
        });

    }

}

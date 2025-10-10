using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TipoLogroController : ControllerBase
{

    private readonly ITipoLogroLogica _tipoLogroLogica;

    public TipoLogroController(ITipoLogroLogica itll)
    {
        this._tipoLogroLogica = itll;
    }

    [HttpGet]
    public IActionResult Listado()
    {
        var tiposLogros = this._tipoLogroLogica.ObtenerTiposLogro();
        return Ok(new
        {
            mensaje = "Todo OK",
            exito = true,
            status = 200,
            data = tiposLogros
        });
    }

    [HttpPost]
    public IActionResult Guardar([FromBody] TipoLogroDTO nuevoTipoLogro)
    {
        if (nuevoTipoLogro == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });

        var tipoLogroGuardado = _tipoLogroLogica.Guardar(nuevoTipoLogro);
        return Ok(tipoLogroGuardado);
    }

    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id) 
    {
        this._tipoLogroLogica.Eliminar(id);
        return Ok(new
        {
            mensaje = "Todo OK",
            exito = true,
            status = 200,
            data = 1
        });

    }
}
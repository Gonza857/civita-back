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
        return Ok(tiposLogros);
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
        return Ok();

    }

    [HttpGet("{id}")]
    public IActionResult getTipoLogroPorId(int id)
    {
        var tipoLogro = this._tipoLogroLogica.ObtenerPorId(id);
        return Ok(tipoLogro);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchTipoLogro([FromBody] TipoLogroDTO tipoLogroDTO, int id)
    {
        try
        {
            this._tipoLogroLogica.Actualizar(tipoLogroDTO, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }

    }
}
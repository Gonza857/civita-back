using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoLogroController : ControllerBase
{

    private readonly ITipoLogroLogica _tipoLogroLogica;

    public TipoLogroController(ITipoLogroLogica itll)
    {
        this._tipoLogroLogica = itll;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var tiposLogros = await this._tipoLogroLogica.ObtenerTiposLogro();
            return Ok(tiposLogros);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el listado de Tipos de Logros");
        }

    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] TipoLogroDTO? nuevoTipoLogro)
    {
        if (nuevoTipoLogro == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });

        try
        {
            var tipoLogroGuardado = await _tipoLogroLogica.Guardar(nuevoTipoLogro);
            return Ok(tipoLogroGuardado);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al guardar el Tipo de Logro");

        }
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id) 
    {
        try
        {
            await this._tipoLogroLogica.Eliminar(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al eliminar el Tipo de Logro");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTipoLogroPorId(int id)
    {
        try
        {
            var tipoLogro = await this._tipoLogroLogica.ObtenerPorId(id);
            return Ok(tipoLogro);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el Tipo de Logro");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTipoLogro([FromBody] TipoLogroDTO? tipoLogroDTO, int id)
    {
        if (tipoLogroDTO == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });
        
        try
        {
            await this._tipoLogroLogica.Actualizar(tipoLogroDTO, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al actualizar el Tipo de Logro");
        }

    }
}
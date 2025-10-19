using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoEstructuraController : ControllerBase
{
    private readonly ITipoEstructuraLogica _tipoEstructuraLogica;

    public TipoEstructuraController(ITipoEstructuraLogica tel)
    {
        this._tipoEstructuraLogica = tel;
    }
    
    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var tiposEstructura = await this._tipoEstructuraLogica.Listado();
            return Ok(tiposEstructura);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el listado de Tipos de Estructura");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] TipoEstructuraDTO? nuevoTipoEstructura)
    {
        if (nuevoTipoEstructura == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });

        try
        {
            TipoEstructuraDTO guardado = await _tipoEstructuraLogica.Crear(nuevoTipoEstructura);
            return Ok(guardado);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al guardar el Tipo de Estructura");

        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id) 
    {
        try
        {
            await this._tipoEstructuraLogica.Eliminar(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al eliminar el Tipo de Estructura");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorId(int id)
    {
        try
        {
            var tipoEstructura = await this._tipoEstructuraLogica.ObtenerPorId(id);
            return Ok(tipoEstructura);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el Tipo de Estructura");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Actualizar([FromBody] TipoEstructuraDTO? tipoEstructuraDto, int id)
    {
        if (tipoEstructuraDto == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });
        
        try
        {
            await this._tipoEstructuraLogica.Actualizar(tipoEstructuraDto, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al actualizar el Tipo de Estructura");
        }
    }

}
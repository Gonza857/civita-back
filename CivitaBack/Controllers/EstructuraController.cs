using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstructuraController : ControllerBase
{
    private readonly IEstructuraLogica _estructuraLogica;
    public EstructuraController(IEstructuraLogica el)
    {
        this._estructuraLogica = el;
    }
    
    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var estructuras = await _estructuraLogica.ObtenerListado();
            return Ok(estructuras);
        }
        catch (Exception)
        {
            return Problem("Ocurrió un error al obtener el listado de Estructuras.");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] EstructuraDTO? estructura)
    {
        if (estructura == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });
        try
        {
            await _estructuraLogica.Crear(estructura);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al guardar la Estructura");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _estructuraLogica.Eliminar(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al eliminar la Estructura.");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorId(int id)
    {
        try
        {
            var estructura = await this._estructuraLogica.ObtenerPorId(id);
            return Ok(estructura);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener la Estructura.");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Actualizar([FromBody] EstructuraDTO? estructuraDTO, int id)
    {
        if (estructuraDTO == null)
            return BadRequest("Los datos recibidos son inválidos");
        
        try
        {
            await this._estructuraLogica.Actualizar(estructuraDTO, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al actualizar la Estructura.");
        }
    }
}
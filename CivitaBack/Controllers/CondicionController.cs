using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CondicionController: ControllerBase
{
    private readonly ICondicionLogica _condicionLogica;
    
    public CondicionController(ICondicionLogica icl)
    {
        this._condicionLogica = icl;
    }
    
    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var condiciones = await _condicionLogica.ObtenerListado();
            return Ok(condiciones);
        }
        catch (Exception)
        {
            return Problem("Ocurrió un error al obtener el listado de Condiciones.");
        }
    }
    

    [HttpGet("Recompensa")]
    public async Task<IActionResult> RecompensaListado()
    {
        try
        {
            var recompensas = await _condicionLogica.ObtenerListadoRecompensas();
            return Ok(recompensas);
        }
        catch (Exception)
        {
            return Problem("Ocurrió un error al obtener el listado de Recompensas.");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CondicionDTO? condicionNueva)
    {
        if (condicionNueva == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });
        try
        {
            await _condicionLogica.Crear(condicionNueva);
            return Ok();
        }
        catch (CondicionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al guardar la Condicion");
        }
    }
    
    [HttpPost("Recompensa")]
    public async Task<IActionResult> CrearRecompensa([FromBody] CondicionDTO? recompensaNueva)
    {
        if (recompensaNueva == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });
        try
        {
            await _condicionLogica.CrearRecompensa(recompensaNueva);
            return Ok();
        }
        catch (CondicionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al guardar la Recompensa");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _condicionLogica.Eliminar(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al eliminar la Condicion.");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCondicionPorId(int id)
    {
        try
        {
            var condicion = await this._condicionLogica.ObtenerPorId(id);
            return Ok(condicion);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener la Condición.");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCondicion([FromBody] CondicionDTO? condicionDto, int id)
    {
        if (condicionDto == null)
            return BadRequest("Los datos recibidos son inválidos");

        try
        {
            await this._condicionLogica.Actualizar(condicionDto, id);
            return Ok();
        }
        catch (CondicionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al actualizar la Condición");
        }
    }

}
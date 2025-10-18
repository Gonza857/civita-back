using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TipoTipController : ControllerBase
{
    private readonly ITipoTipLogica _tipoTipLogica;
    
    public TipoTipController(ITipoTipLogica ttl)
    {
        this._tipoTipLogica = ttl;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var tiposTip = await this._tipoTipLogica.Listado();
            return Ok(tiposTip);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el listado de Tipos de Tips");
        }

    }
    
    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] TipoTipDTO? nuevoTipTipDto)
    {
        if (nuevoTipTipDto == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });

        try
        {
            await _tipoTipLogica.Guardar(nuevoTipTipDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al guardar el Tipo de Tip");
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id) 
    {
        try
        {
            await this._tipoTipLogica.Eliminar(id);
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
            var tipoLogro = await this._tipoTipLogica.ObtenerPorId(id);
            return Ok(tipoLogro);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al obtener el Tipo de Logro");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTipoTip([FromBody] TipoTipDTO? tipoTipDto, int id)
    {
        if (tipoTipDto == null)
            return BadRequest(new { mensaje = "Los datos recibidos son inválidos" });
        
        try
        {
            await this._tipoTipLogica.Actualizar(tipoTipDto, id);
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al actualizar el Tipo de Logro");
        }
    }
}
    


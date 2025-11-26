using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class CondicionController: BaseApiController
{
    private readonly ICondicionLogica _condicionLogica;
    private readonly ILogger<CondicionController> _logger;
    public CondicionController(ICondicionLogica icl, IMapper mapper, ILogger<CondicionController> logger) : base(mapper)
    {
        this._condicionLogica = icl;
        this._logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var condiciones = await _condicionLogica.ObtenerListado();
            var condicionesDto = base.MapearLista<CondicionDTO>(condiciones);
            return Ok(condicionesDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Condiciones.");
        }
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CondicionDTO? condicionDto)
    {
        try
        {
            var condicionEntidad = base.Mapear<Condicion>(condicionDto);

            await _condicionLogica.Crear(condicionEntidad, condicionDto.RecompensaIds);
            return Ok();
        }
        catch (CondicionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al guardar la Condicion");
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
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al eliminar la Condicion.");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCondicionPorId(int id)
    {
        try
        {
            var condicion = await this._condicionLogica.ObtenerPorId(id);
            var condicionDto = base.Mapear<CondicionDTO>(condicion);
            return Ok(condicionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener la Condición.");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCondicion([FromBody] ActualizarCondicionDTO condicionDto, int id)
    {
        try
        {
            var condicionEntidad = base.Mapear<Condicion>(condicionDto);
            await this._condicionLogica.Actualizar(condicionEntidad, id, condicionDto.RecompensaIds);
            return Ok();
        }
        catch (CondicionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al actualizar la Condición");
        }
    }

}
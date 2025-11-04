using AutoMapper;
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
    

    [HttpGet("Recompensa")]
    public async Task<IActionResult> RecompensaListado()
    {
        try
        {
            var recompensas = await _condicionLogica.ObtenerListadoRecompensas();
            var recompensasDto = base.MapearLista<CondicionDTO>(recompensas);
            return Ok(recompensasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            var condicionEntidad = base.Mapear<Condicion>(condicionNueva);

            await _condicionLogica.Crear(condicionEntidad);
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
    
    [HttpPost("Recompensa")]
    public async Task<IActionResult> CrearRecompensa([FromBody] CondicionDTO? recompensaNueva)
    {
        if (recompensaNueva == null)
            return BadRequest("Los datos recibidos son inválidos");
        try
        {
            var recompensaEntidad = base.Mapear<Condicion>(recompensaNueva);
            await _condicionLogica.CrearRecompensa(recompensaEntidad);
            return Created();
        }
        catch (CondicionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
    public async Task<IActionResult> PatchCondicion([FromBody] CondicionDTO? condicionDto, int id)
    {
        if (condicionDto == null)
            return BadRequest("Los datos recibidos son inválidos");

        try
        {
            var condicionEntidad = base.Mapear<Condicion>(condicionDto);

            await this._condicionLogica.Actualizar(condicionEntidad, id);
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
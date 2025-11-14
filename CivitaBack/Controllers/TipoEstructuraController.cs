using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class TipoEstructuraController : BaseApiController
{
    private readonly ITipoEstructuraLogica _tipoEstructuraLogica;
    private readonly ILogger<TipoEstructuraController> _logger;

    public TipoEstructuraController(ITipoEstructuraLogica tel, IMapper mapper, ILogger<TipoEstructuraController> logger) : base(mapper)
    {
        this._tipoEstructuraLogica = tel;
        this._logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var tiposEstructura = await this._tipoEstructuraLogica.Listado();
            return Ok(base.MapearLista<TipoEstructuraDTO>(tiposEstructura));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            var guardado = await _tipoEstructuraLogica.Crear(base.Mapear<TipoEstructura>(nuevoTipoEstructura));
            return Ok(base.Mapear<TipoEstructuraDTO>(guardado));
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al eliminar el Tipo de Estructura");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorId(int id)
    {
        try
        {
            var tipoEstructura = await this._tipoEstructuraLogica.ObtenerPorId(id);
            return Ok(base.Mapear<TipoEstructuraDTO>(tipoEstructura));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el Tipo de Estructura");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> Actualizar([FromBody] TipoEstructuraDTO? tipoEstructuraDto, int id)
    {
        if (tipoEstructuraDto == null)
            return BadRequest("Los datos recibidos son inválidos");
        
        try
        {
            var estructura = base.Mapear<TipoEstructura>(tipoEstructuraDto);
            await this._tipoEstructuraLogica.Actualizar(estructura, id);
            return Ok();
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al actualizar el Tipo de Estructura");
        }
    }

}
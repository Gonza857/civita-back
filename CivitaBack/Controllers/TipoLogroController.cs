using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using CivitaBack.Logica.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;


[Route("api/[controller]")]
public class TipoLogroController : BaseApiController
{

    private readonly ITipoLogroLogica _tipoLogroLogica;
    private readonly ILogger<TipoLogroController> _logger;

    public TipoLogroController(ITipoLogroLogica itll, ILogger<TipoLogroController> logger,IMapper mapper) : base(mapper)
    {
        this._tipoLogroLogica = itll;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var tiposLogros = await this._tipoLogroLogica.ObtenerTiposLogro();
            return Ok(base.MapearLista<TipoLogroDTO>(tiposLogros));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Tipos de Logros");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] TipoLogroDTO? nuevoTipoLogro)
    {
        if (nuevoTipoLogro == null)
            return BadRequest("Los datos recibidos son inválidos");

        try
        {
            var tipoLogroGuardado = await _tipoLogroLogica.Guardar(base.Mapear<TipoLogro>(nuevoTipoLogro));
            return Ok(base.Mapear<TipoLogroDTO>(tipoLogroGuardado));
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al eliminar el Tipo de Logro");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTipoLogroPorId(int id)
    {
        try
        {
            var tipoLogro = await this._tipoLogroLogica.ObtenerPorId(id);
            return Ok(base.Mapear<TipoLogroDTO>(tipoLogro));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el Tipo de Logro");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTipoLogro([FromBody] TipoLogroDTO? tipoLogroDTO, int id)
    {
        if (tipoLogroDTO == null)
            return BadRequest("Los datos recibidos son inválidos");
        
        try
        {
            await this._tipoLogroLogica.Actualizar(base.Mapear<TipoLogro>(tipoLogroDTO), id);
            return Ok();
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al actualizar el Tipo de Logro");
        }
    }
}
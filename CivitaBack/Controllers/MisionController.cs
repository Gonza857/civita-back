using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class MisionController : BaseApiController
{
    private readonly IMisionLogica _misionLogica;
    private readonly ILogger<MisionController> _logger;
    public MisionController(
        IMisionLogica iml,
        IMapper mapper,
        ILogger<MisionController> logger) : base(mapper)
    {
        _misionLogica = iml;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var misiones = await _misionLogica.Listado();
            return Ok(base.MapearLista<MisionDTO>(misiones));
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Misiones.");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] MisionDTO? nuevaMisionDto)
    {
        if (nuevaMisionDto == null)
            return BadRequest("Los datos recibidos son inválidos");
        try
        {
            Mision misionNueva = base.Mapear<Mision>(nuevaMisionDto);
            await _misionLogica.Crear(misionNueva);
            return Ok();
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (LogroExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al guardar el Logro");
        }
    }
    
    //
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Eliminar(int id)
    // {
    //     try
    //     {
    //         await _logroLogica.Eliminar(id);
    //         return Ok();
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex.Message);
    //         return Problem("Ocurrió un error al eliminar el logro.");
    //     }
    // }
    //
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMisionPorId(int id)
    {
        try
        {
            var mision = await this._misionLogica.ObtenerPorId(id);
            return Ok(base.Mapear<MisionDTO>(mision));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener la mision.");
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchMision([FromBody] MisionDTO? misionDto, int id)
    {
        if (misionDto == null)
            return BadRequest("Los datos recibidos son inválidos");

        try
        {
            var mision = base.Mapear<Mision>(misionDto);
            await this._misionLogica.Actualizar(mision, id);
            return Ok();
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (MisionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al actualizar la Misión");
        }
    }

}

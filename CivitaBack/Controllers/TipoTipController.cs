using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class TipoTipController : BaseApiController
{
    private readonly ITipoTipLogica _tipoTipLogica;
    private readonly ILogger<TipoLogroController> _logger;

    public TipoTipController(ITipoTipLogica ttl, IMapper mapper, ILogger<TipoLogroController> logger) : base(mapper)
    {
        this._tipoTipLogica = ttl;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Listado()
    {
        try
        {
            var tiposTip = await this._tipoTipLogica.Listado();
            return Ok(base.MapearLista<TipoTipDTO>(tiposTip));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            await _tipoTipLogica.Guardar(base.Mapear<TipoTip>(nuevoTipTipDto));
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al eliminar el Tipo de Logro");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTipoTipPorId(int id)
    {
        try
        {
            var tipoTip = await this._tipoTipLogica.ObtenerPorId(id);
            if (tipoTip == null) return NotFound();
            return Ok(base.Mapear<TipoTip>(tipoTip));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el Tipo de Logro");
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTipoTip([FromBody] TipoTipDTO? tipoTipDto, int id)
    {
        if (tipoTipDto == null)
            return BadRequest("Los datos recibidos son inválidos");

        try
        {
            await this._tipoTipLogica.Actualizar(base.Mapear<TipoTip>(tipoTipDto), id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al actualizar el Tipo de Logro");
        }
    }
}
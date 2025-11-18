using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class RecompensaController : BaseApiController
{
    private readonly IRecompensaLogica _recompensaLogica;
    private readonly ILogger<RecompensaController> _logger;
    
    public RecompensaController(
        IRecompensaLogica icl, 
        IMapper mapper,
        ILogger<RecompensaController> logger) : base (mapper)
    {
        this._recompensaLogica = icl;
        this._logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> RecompensaListado()
    {
        try
        {
            var recompensas = await _recompensaLogica.Listado();
            var recompensasDto = base.MapearLista<RecompensaDTO>(recompensas);
            return Ok(recompensasDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Recompensas.");
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CrearRecompensa([FromBody] RecompensaDTO? recompensaNueva)
    {
        try
        {
            var recompensaEntidad = base.Mapear<Recompensa>(recompensaNueva);
            await _recompensaLogica.CrearRecompensa(recompensaEntidad);
            return Created();
        }
        catch (DominioException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al guardar la Recompensa");
        }
    }
    
}
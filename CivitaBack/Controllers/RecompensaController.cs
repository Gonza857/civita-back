using AutoMapper;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class RecompensaController : BaseApiController
{
    private readonly ICondicionLogica _condicionLogica;
    private readonly ILogger<RecompensaController> _logger;
    
    public RecompensaController(
        ICondicionLogica icl, 
        IMapper mapper,
        ILogger<RecompensaController> logger) : base (mapper)
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
            return Ok(base.MapearLista<CondicionDTO>(condiciones));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Condiciones.");
        }
    }
}
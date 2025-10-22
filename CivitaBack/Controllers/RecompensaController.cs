using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecompensaController : ControllerBase
{
    private readonly ICondicionLogica _condicionLogica;
    
    public RecompensaController(ICondicionLogica icl)
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
}
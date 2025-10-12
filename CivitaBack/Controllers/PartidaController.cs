using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;
using CivitaBack.Data.DTO; // 🆕 Para usar el DTO de guardado
using System.Threading.Tasks;

namespace CivitaBack.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PartidaController : ControllerBase
{
    private readonly IPartidaLogica _partidaServicio;

    public PartidaController(IPartidaLogica pl)
    {
        this._partidaServicio = pl;
    }

    [HttpPost]
    public IActionResult PostPartida()
    {
        var partida = this._partidaServicio.CrearPartida();
        return Ok(partida);
    }

    [HttpGet]
    public IActionResult GetPartidas()
    {
        var partidas = this._partidaServicio.ObtenerPartidas();
        return Ok(partidas);
    }

    [HttpPost("guardar-mapa")]
    public async Task<IActionResult> GuardarMapa([FromBody] GuardarMapaDTO dto)
    {
        if (dto == null || dto.PartidaId <= 0)
            return BadRequest("Datos de mapa inválidos.");

        try
        {
            await _partidaServicio.GuardarMapaAsync(dto);
            return Ok(new { mensaje = "Mapa guardado correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al guardar el mapa.", detalle = ex.Message });
        }
    }


    [HttpGet("{partidaId}/mapa")]
    public async Task<IActionResult> ObtenerMapa(int partidaId)
    {
        try
        {
            var mapa = await _partidaServicio.ObtenerMapaAsync(partidaId);
            if (mapa == null)
                return NotFound(new { mensaje = "No se encontró el mapa de la partida." });

            return Ok(mapa);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al obtener el mapa.", detalle = ex.Message });
        }
    }

}

using CivitaBack.Data.BO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

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
        return Ok(new
        {
            mensaje = "Todo OK",
            exito = true,
            status = 200,
            data = partidas
        });
    }
}

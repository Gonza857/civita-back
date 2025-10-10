using CivitaBack.Data.BO;
using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PartidaController : ControllerBase
{

    private readonly IPartidaLogica _partidaLogica;
    private readonly IRecursoLogica _recursoLogica;

    public PartidaController(IPartidaLogica pl, IRecursoLogica rl)
    {
        this._partidaLogica = pl;
        this._recursoLogica = rl;
    }

    [HttpPost("Iniciar/{idUsuario}")]
    public IActionResult Iniciar(int idUsuario)
    {
        try
        {
            Partida partida = this._partidaLogica.CrearPartida(7);
            this._recursoLogica.ConfigurarInicial(partida);
            return Ok(partida);
        } catch (ErrorInternoExcepction e)
        {
            return Problem("Ocurrió un error al guardar");
        }
    }

    [HttpGet]
    public IActionResult GetPartidas()
    {
        var partidas = this._partidaLogica.ObtenerPartidas();
        return Ok(new
        {
            mensaje = "Todo OK",
            exito = true,
            status = 200,
            data = partidas
        });
    }

    [HttpGet("{id}")]
    public IActionResult GetPartidaPorId(int id)
    {
        var partida = this._partidaLogica.ObtenerPorUsuarioId(id);
        return Ok(new
        {
            mensaje = "Todo OK",
            exito = true,
            status = 200,
            data = partida
        });
    }
}

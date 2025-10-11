using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
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
    private readonly IAuthLogica _authLogica;

    public PartidaController(IPartidaLogica pl, IRecursoLogica rl, IAuthLogica al)
    {
        this._partidaLogica = pl;
        this._recursoLogica = rl;
        this._authLogica = al;
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
        return Ok(partidas);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchPartida([FromBody] PartidaDTO partidaDTO, int id)
    {
        try
        {
            Usuario usuario = await this._authLogica.ObtenerPorId(partidaDTO.UsuarioId);
            this._partidaLogica.Actualizar(partidaDTO, usuario);
            return Ok();
        } catch (PartidaExcepcion ex) {
            return Conflict(new { message = ex.Message });
        } catch (Exception ex) {
            return Problem("Ocurrió un error en el servidor.");
        }

    }

    [HttpGet("{id}")]
    public IActionResult GetPartidaPorId(int id)
    {
        var partida = this._partidaLogica.ObtenerPorUsuarioId(id);
        return Ok(partida);
    }
}

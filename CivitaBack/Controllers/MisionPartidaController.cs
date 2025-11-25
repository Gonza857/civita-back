using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/Mision")]
public class MisionPartidaController : BaseApiController
{
    private readonly IPartidaLogica _partidaLogica;
    private readonly IMisionLogica _misionLogica;
    private readonly IMisionPartidaLogica _misionPartidaLogica;
    private readonly IRecompensaLogica _recompensaLogica;
    private readonly ICondicionLogica _condicionLogica;
    private readonly INivelLogica _nivelLogica;

    private readonly ILogger<MisionController> _logger;
    
    public MisionPartidaController(
        IPartidaLogica partidaLogica,
        IMisionLogica misionLogica,
        IMisionPartidaLogica misionPartidaLogica,
        IRecompensaLogica recompensaLogica,
        ICondicionLogica condicionLogica,
        ILogger<MisionController> logger,
        IMapper mapper,
        INivelLogica nivelLogica
        ) : base(mapper)
    {
        this._partidaLogica = partidaLogica;
        this._misionLogica = misionLogica;
        this._misionPartidaLogica = misionPartidaLogica;
        this._recompensaLogica = recompensaLogica;
        this._condicionLogica = condicionLogica;
        this._logger = logger;
        this._nivelLogica = nivelLogica;
    }
    
    // Asignar misiones disponible a una partida
    [HttpGet("Iniciar/{idPartida}")]
    public async Task<IActionResult> EntregarMisiones(int idPartida)
    {
        try
        {
            Partida partida = await this._partidaLogica.ObtenerPorId(idPartida);
            var misionesActivas = await this._misionLogica.ObtenerMisionesDisponibles();
            await this._misionPartidaLogica.AsignarMisiones(misionesActivas, partida);
            return Ok("Misiones asignadas a la partida");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al asignar misiones a una partida.");
        }
    }
    
    // Obtener recompensa de una mision única
    [HttpGet("Reclamar/{idPartida}/{idMision}")]
    public async Task<IActionResult> ObtenerMisionesDisponibles(int idPartida, int idMision)
    {
        try
        {
            Partida partida = await this._partidaLogica.ObtenerPorId(idPartida);
            var mision = await this._misionPartidaLogica.ObtenerMisionPartidaPorId(partida.Id, idMision);
            var condicionesQueCumple = this._condicionLogica.FiltrarCondicionSiCumple(mision.Condicion, partida);
            
            if (condicionesQueCumple.Count == 0)
                return BadRequest("No tenes misiones listas para reclamar");
            
            await this._recompensaLogica.ReclamarRecompensas(mision.Condicion.Recompensas.ToList(), partida);
            await this._misionPartidaLogica.MarcarMisionCompletada(mision, partida);
            // await this._nivelLogica
            // await _nivelLogica.
            
            return Ok();
        }
        catch (CondicionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Misiones.");
        }
    }
    
    // Misiones que tiene para cumplir el usuario en su partida (diarias, semanales y mensuales)
    [HttpGet("Activas/{idPartida}")]
    public async Task<IActionResult> ObtenerMisionesDisponibles(int idPartida)
    {
        try
        {
            Partida? partida = await this._partidaLogica.ObtenerPorId(idPartida);
            var misiones = await _misionLogica.ObtenerMisionesActivasParaPartida(partida);
            return Ok(base.MapearLista<MisionDTO>(misiones));
        }
        catch (MisionExcepcion ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al obtener el listado de Misiones.");
        }
    }
}
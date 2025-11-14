using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/Partida")]
public class LogroPartidaController : BaseApiController
{
    private readonly ILogroPartidaLogica _logroPartidaLogica;
    private readonly IPartidaLogica _partidaLogica;

    public LogroPartidaController(ILogroPartidaLogica lpl, IPartidaLogica ipl, IMapper mapper) : base(mapper)
    {
        this._logroPartidaLogica = lpl;
        this._partidaLogica = ipl;
    }

    [HttpGet("{idUsuario}/Reclamar")]
    public async Task<IActionResult> ReclamarLogros(int idUsuario)
    {
        try
        {
            Partida? partida = await this._partidaLogica.ObtenerPartidaPorIdInterno(idUsuario);
            await _logroPartidaLogica.ReclamarLogros(partida);
            return Ok();
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (Exception e)
        {
            return Problem("Ocurrió un error al reclamar los logros");
        }
    }

    [HttpGet("{idUsuario}/Logros")]
    public async Task<IActionResult> ObtenerLogrosPartida(int idUsuario, [FromQuery] string? status)
    {
        try
        {
            Partida? partida = await this._partidaLogica.ObtenerPartidaPorIdInterno(idUsuario);

            var logros = await (status?.ToLower().Trim() switch
            {
                // 1. No obtenidos
                "incompletos" => _logroPartidaLogica.ObtenerLogrosIncompletos(partida),

                // 2. Para reclamar (completos pero no cobrados)
                "para_reclamar" => _logroPartidaLogica.ObtenerLogrosParaReclamar(partida),
                
                // 4. Caso por defecto (si status es null o string vacío)
                null or "" or "reclamados" => _logroPartidaLogica.ObtenerLogrosCompletados(partida),

                // 5. El "Churrasco"
                _ => throw new ArgumentException($"El status '{status}' no es válido.")
            });

            return Ok(base.MapearLista<LogroDTO>(logros));
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (ArgumentException ex) // Captura el "churrasco"
        {
            // Esto devuelve un 400 Bad Request
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "Error al obtener logros para partida {PartidaId}", partidaId);
            // Esto devuelve un 500 Internal Server Error
            return Problem("Error al obtener logros");
        }
    }
}

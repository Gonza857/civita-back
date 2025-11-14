using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    public class TipController : BaseApiController
    {
        private readonly ITipLogica _tipLogica;
        private readonly ILogger<TipController> _logger;

        public TipController(ITipLogica itl, IMapper mapper, ILogger<TipController> logger) : base(mapper)
        {
            this._tipLogica = itl;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Listado()
        {
            try
            {
                var tips = await this._tipLogica.Listado();
                return Ok(base.MapearLista<TipDTO>(tips));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error al obtener los Tips.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var tip = await this._tipLogica.ObtenerPorIdTipo(id);
                if (tip == null) return NotFound();
                return Ok(base.Mapear<TipDTO>(tip));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error al actualizar el Tip.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Actualizar([FromBody] TipDTO? tipDto, int id)
        {
            try
            {
                var tip = base.Mapear<Tip>(tipDto);
                await this._tipLogica.Actualizar(tip, id);
                return Ok();
            }
            catch (AccesoDenegadoExcepcion ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error al actualizar el Tip.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] TipDTO? tipDto)
        {
            if (tipDto == null)
                return BadRequest("No se proporcionó Tip");
            try
            {
                await this._tipLogica.Crear(base.Mapear<Tip>(tipDto));
                return Created();
            }
            catch (AccesoDenegadoExcepcion ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un error al crear el Tip.");
            }
        }

        [HttpGet("ObtenerMensajesPorIdTipo")]
        public async Task<IActionResult> ObtenerMensajesPorIdTipo(int id)
        {
            try
            {
                var listaMensajes = await this._tipLogica.ObtenerMsjPorIdTipo(id);
                return Ok(base.MapearLista<TipDTO>(listaMensajes));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem("Ocurrió un problema al obtener los mensajes por un tipo");
            }
        }
    }
}
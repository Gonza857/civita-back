using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipController : ControllerBase
    {
        private readonly ITipsLogica _tipLogica;

        public TipController (ITipsLogica itl)
        {
            this._tipLogica =itl;
        }

        [HttpGet]
        public async Task<IActionResult> Listado()
        {
            try
            {
                var tips = await this._tipLogica.Listado();
                return Ok(tips);
            }
            catch (Exception e)
            {
                return Problem("Ocurrió un error al obtener los Tips.");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            if (id <= 0)
                return BadRequest("No se proporcionó id");
            try
            {
                var tip = await this._tipLogica.ObtenerPorIdTipo(id);
                if (tip == null) return NotFound();
                return Ok(tip);
            }
            catch (Exception e)
            {
                return Problem("Ocurrió un error al actualizar el Tip.");
            }
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> Actualizar([FromBody]TipDTO? tipDto, int id)
        {
            if (tipDto == null || id <= 0)
                return BadRequest("No se proporcionó Tip");
            try
            {
                await this._tipLogica.Actualizar(tipDto, id);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem("Ocurrió un error al actualizar el Tip.");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody]TipDTO? tipDto)
        {
            if (tipDto == null)
                return BadRequest("No se proporcionó Tip");
            try
            {
                await this._tipLogica.Crear(tipDto);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem("Ocurrió un error al crear el Tip.");
            }
        }
        
        [HttpGet("ObtenerMensajesPorIdTipo")]
        public async Task<IActionResult> ObtenerMensajesPorIdTipo(int id)
        {
            try
            {
                var listaMensajes = await this._tipLogica.ObtenerMsjPorIdTipo(id);
                return Ok(listaMensajes);
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió un problema al obtener los mensajes por un tipo");
            }
  
        }
    }
}

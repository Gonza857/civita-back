using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Interfaces.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    public class TiendaController : BaseApiController
    {

        private readonly IEstructuraLogica _estructuraLogica;

        public TiendaController(IEstructuraLogica estructuraLogica, IMapper mapper) : base(mapper)
        {
            _estructuraLogica = estructuraLogica;
        }

        [HttpGet("estructuras")]
        public async Task<IActionResult> ObtenerEstructurasParaTienda()
        {
            try
            {
                var estructuras = await _estructuraLogica.ObtenerListado();

                List<TiendaDTO> tienda = base.MapearLista<TiendaDTO>(estructuras);

                return Ok(tienda);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al obtener las estructuras.");
            }
        }


    }
}

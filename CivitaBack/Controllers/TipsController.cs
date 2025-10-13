using CivitaBack.Logica;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipsController : ControllerBase
    {
        // GET: TipsController
        private readonly ITipsLogica tipsLogica;    


        public TipsController (ITipsLogica itl)
        {
            this.tipsLogica =itl;
        }
        
        
        [HttpGet("ObtenerMensajesPorIdTipo")]
        public IActionResult ObtenerMensajesPorIdTipo(int id)
        {
            var listaMensajes = this.tipsLogica.ObtenerMsjPorIdTipo(id);
            return Ok(listaMensajes);
        }

        
        
        [HttpGet("ObtenerTiposTips")]
        public IActionResult ObtenerTiposTips()
        {
            var listaTipos = this.tipsLogica.GetTiposTips(); 
            return Ok(listaTipos);
        }

    }
}

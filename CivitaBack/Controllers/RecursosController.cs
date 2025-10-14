using CivitaBack.Data.DTO;
using CivitaBack.Logica;
using CivitaBack.Logica.Excepciones;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursosController : ControllerBase
    {

        private readonly IRecursoLogica _recursoLogica;
        public RecursosController(IRecursoLogica rl)
        {
            this._recursoLogica = rl;
        }

        [HttpGet("{idPartida}")]
        public IActionResult GetRecursosPartida(int idPartida)
        {
            try
            {
                RecursoDTO recursos = this._recursoLogica.ObtenerRecursos(idPartida);
                return Ok(recursos);
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem("Ocurrió un error");
            }
        }

        //  Aumentar energía
        [HttpPost("subirEnergia/{idPartida}")]
        public IActionResult SubirEnergia(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarEnergia(idPartida, 10);
                return Ok(new { mensaje = "Energía aumentada correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        //  Reducir energía
        [HttpPost("bajarEnergia/{idPartida}")]
        public IActionResult BajarEnergia(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarEnergia(idPartida, -10);
                return Ok(new { mensaje = "Energía reducida correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        // Subir Felicidad
        [HttpPost("subirFelicidad/{idPartida}")]
        public IActionResult SubirFelicidad(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarFelicidad(idPartida, 10);
                return Ok(new { mensaje = "Felicidad aumentada correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        //  Reducir felicidad
        [HttpPost("bajarFelicidad/{idPartida}")]
        public IActionResult BajarFelicidad(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarFelicidad(idPartida, -10);
                return Ok(new { mensaje = "Felicidad reducida correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        // Subir Contaminacion
        [HttpPost("subirContaminacion/{idPartida}")]
        public IActionResult SubirContaminacion(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarContaminacion(idPartida, 10);
                return Ok(new { mensaje = "Contaminacion aumentada correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        //  Reducir Contaminacion
        [HttpPost("bajarContaminacion/{idPartida}")]
        public IActionResult BajarContaminacion(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarContaminacion(idPartida, -10);
                return Ok(new { mensaje = "Contaminacion reducida correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        // Subir Ecocoins
        [HttpPost("subirEcocoins/{idPartida}")]
        public IActionResult SubirEcocoins(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarEcocoins(idPartida, 1);
                return Ok(new { mensaje = "Ecocoins aumentada correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }

        //  Reducir Ecocoins
        [HttpPost("bajarEcocoins/{idPartida}")]
        public IActionResult BajarEcocoins(int idPartida)
        {
            try
            {
                _recursoLogica.ModificarEcocoins(idPartida, -10);
                return Ok(new { mensaje = "Ecocoins reducida correctamente." });
            }
            catch (PartidaExcepcion ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return Problem("Ocurrió un error al modificar la energía.");
            }
        }
    }
}

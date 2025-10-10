using CivitaBack.Logica;
using CivitaBack.Data;
using Microsoft.AspNetCore.Mvc;
using CivitaBack.Data.DTO;


namespace CivitaBack.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthLogica _authLogica;

        public AuthController(IAuthLogica authLogica)
        {
            _authLogica = authLogica;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] RegistroDto request)
        {
            try
            {
                var usuario = await _authLogica.RegistrarUsuarioAsync(request.NombreUsuario, request.Mail, request.Password);
                return Ok(new { mensaje = "Usuario registrado correctamente!", usuario });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error interno del servidor." });
            }
        }
    }

}

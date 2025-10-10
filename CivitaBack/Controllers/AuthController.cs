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
                return Ok(new { usuario.Id, usuario.NombreUsuario, usuario.Mail });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}

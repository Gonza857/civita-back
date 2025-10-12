using CivitaBack.Logica;
using CivitaBack.Data;
using Microsoft.AspNetCore.Mvc;
using CivitaBack.Data.DTO;
using CivitaBack.Logica.Excepciones;


namespace CivitaBack.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthLogica _authLogica;
        private readonly IPartidaLogica _partidaLogica;

        public AuthController(IAuthLogica authLogica , IPartidaLogica partidaLogica)
        {
            _authLogica = authLogica;
            _partidaLogica = partidaLogica;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] RegistroDto request)
        {
            try
            {
                var usuario = await _authLogica.RegistrarUsuarioAsync(request.NombreUsuario, request.Mail, request.Password);

                _partidaLogica.CrearPartida(usuario.Id);

                return Ok(new { mensaje = "Usuario registrado correctamente!", usuario });
            }
            catch (ValidacionRegistroException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error interno del servidor." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var resultado = await _authLogica.LoginAsync(request);

                var partidaDTO = _partidaLogica.ObtenerPorUsuarioId(resultado.IdUsuario);

                var response = new LoginResponse
                {
                    Token = resultado.Token,
                    NombreUsuario = resultado.NombreUsuario,
                    Mail = resultado.Mail,
                    IdUsuario = resultado.IdUsuario,
                    Partida = partidaDTO
                };

                return Ok(response);
            } catch (AutenticacionException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error interno del servidor." });
            }
          
        }

        [HttpGet("existeNombre")]
        public async Task<IActionResult> ExisteNombre([FromQuery] string nombre)
        {
            var existe = await _authLogica.ObtenerUsuarioPorNombre(nombre) != null;
            return Ok(new { existe = true });
        }
    }

}

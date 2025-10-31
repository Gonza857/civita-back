using CivitaBack.Logica;
using CivitaBack.Data;
using Microsoft.AspNetCore.Mvc;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica.Excepciones;


namespace CivitaBack.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthLogica _authLogica;
        private readonly IPartidaLogica _partidaLogica;
        private readonly IUsuarioLogica _usuarioLogica;
        private readonly IRecursoLogica _recursoLogica;

        public AuthController(IAuthLogica authLogica , IPartidaLogica partidaLogica, IUsuarioLogica usuarioLogica, IRecursoLogica recursoLogica)
        {
            _authLogica = authLogica;
            _partidaLogica = partidaLogica;
            _usuarioLogica = usuarioLogica;
            _recursoLogica = recursoLogica;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registrar([FromBody] RegistroDTO request)
        {
            try
            {
                var usuario = await _authLogica.RegistrarUsuarioAsync(request.NombreUsuario, request.Mail, request.Password);

                var partida = await _partidaLogica.CrearPartida(usuario.Id);
                
               await _recursoLogica.ConfigurarInicial(partida);

                return Ok(new { mensaje = "Usuario registrado correctamente!", usuario });
            }
            catch (ValidacionRegistroException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor.", detalle = ex.Message });
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
                    IdPartida = partidaDTO.Id
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
        public async Task<IActionResult> ExisteNombre([FromQuery] string? nombre)
        {
            if (nombre == null) return BadRequest("El nombre del usuario no puede ser nulo.");
            var existe = await _usuarioLogica.ObtenerUsuarioPorNombre(nombre) != null;
            return Ok(new { existe });
        }
    }

}

using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Excepciones;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IAuthLogica _authLogica;
    private readonly IPartidaLogica _partidaLogica;
    private readonly IUsuarioLogica _usuarioLogica;
    private readonly IInicialLogica _inicialLogica;
    private readonly ILogger _logger;

    public AuthController(
        IAuthLogica authLogica,
        IPartidaLogica partidaLogica,
        IUsuarioLogica usuarioLogica,
        IInicialLogica inicialLogica,
        ILogger<AuthController> logger,
        IMapper mapper) : base(mapper)
    {
        _authLogica = authLogica;
        _partidaLogica = partidaLogica;
        _usuarioLogica = usuarioLogica;
        _inicialLogica = inicialLogica;
        _logger = logger;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar([FromBody] RegistroDTO request)
    {
        try
        {
            Usuario usuario = await _authLogica.CrearUsuario(request.NombreUsuario, request.Mail, request.Password);
            await this._inicialLogica.IniciarPartida(usuario);
            Usuario usuarioPartida = await _usuarioLogica.ObtenerPorCorreo(request.Mail);
            Partida partida = usuarioPartida.Partida;

            string token = await _authLogica.IniciarSesion(request.Mail, request.Password);

            var response = new LoginDTO
            {
                NombreUsuario = usuarioPartida.NombreUsuario!,
                Mail = usuarioPartida.Mail!,
                IdUsuario = usuarioPartida.Id!,
                IdPartida = partida.Id
            };

            return Ok(new { mensaje = "Usuario registrado correctamente!", response });
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (ValidacionRegistroException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al realizar el registro.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] IniciarSesionDTO iniciarSesionDto)
    {
        try
        {
            string token = await _authLogica.IniciarSesion(iniciarSesionDto.Mail, iniciarSesionDto.Contrasena);
            Usuario? usuario = await _usuarioLogica.ObtenerPorCorreo(iniciarSesionDto.Mail);

            if (usuario == null)
            {
                throw new AutenticacionException("Error interno de autenticación.");
            }

            Partida? partida = await _partidaLogica.ObtenerPartidaParaLogin(usuario.Id);

            // 🍪 GUARDAR EL TOKEN EN LA COOKIE
            Response.Cookies.Append(
                "jwt-auth", // Nombre de la cookie
                token,
                new CookieOptions
                {
                    HttpOnly = true, // 🛡️ Evita acceso vía JavaScript (XSS)
                    Expires = DateTimeOffset.UtcNow.AddHours(1), // Coincide con la expiración del JWT
                    Secure = true, // Recomendado: Solo para HTTPS
                    SameSite = SameSiteMode.Strict // Protección CSRF
                }
            );

            var response = new LoginDTO
            {
                NombreUsuario = usuario.NombreUsuario!,
                Mail = usuario.Mail!,
                IdUsuario = usuario.Id,
                IdPartida = partida?.Id ?? 0
            };

            return Ok(response);
        }
        catch (AccesoDenegadoExcepcion ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });
        }
        catch (AutenticacionException ex)
        {
            // Si hay fallo de autenticación (usuario/contraseña incorrectos), limpiar cookies y devolver 401
            Response.Cookies.Delete("jwt-auth");
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            Response.Cookies.Delete("jwt-auth");
            return Problem("Ocurrió un error al iniciar sesión");
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

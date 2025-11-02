using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica.Excepciones;
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
            return Ok(new { mensaje = "Usuario registrado correctamente!", usuario });
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
            Usuario usuario = await _usuarioLogica.ObtenerPorCorreo(iniciarSesionDto.Mail);
            Partida partida = await _partidaLogica.ObtenerPorUsuarioId(usuario.Id);

            var response = new LoginDTO
            {
                Token = token,
                NombreUsuario = usuario.NombreUsuario!,
                Mail = usuario.Mail!,
                IdUsuario = usuario.Id!,
                IdPartida = partida.Id
            };

            return Ok(response);
        }
        catch (AutenticacionException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
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

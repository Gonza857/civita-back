using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Excepciones;
using Microsoft.AspNetCore.Mvc;
using CivitaBack.Logica.Interfaces;

namespace CivitaBack.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IAuthLogica _authLogica;
    private readonly IPartidaLogica _partidaLogica;
    private readonly IUsuarioLogica _usuarioLogica;
    private readonly IInicialLogica _inicialLogica;
    private readonly IConfigurarCookieLogica _configurarCookieLogica;
    private readonly ILogger _logger;
    
    private readonly IAccesoUsuarios _accesoUsuarios;

    public AuthController(
        IAuthLogica authLogica,
        IPartidaLogica partidaLogica,
        IUsuarioLogica usuarioLogica,
        IInicialLogica inicialLogica,
        IConfigurarCookieLogica configurarCookieLogica,
        ILogger<AuthController> logger,
        IMapper mapper,
        IAccesoUsuarios iau) : base(mapper)
    {
        _authLogica = authLogica;
        _partidaLogica = partidaLogica;
        _usuarioLogica = usuarioLogica;
        _inicialLogica = inicialLogica;
        _configurarCookieLogica = configurarCookieLogica;
        _logger = logger;
        _accesoUsuarios = iau;
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registrar([FromBody] RegistroDTO request)
    {
        try
        {
            Usuario? usuarioExistente = await this._usuarioLogica.ObtenerUsuarioPorNombre(request.NombreUsuario);
            
            Usuario usuario = await this._authLogica.CrearUsuarioInicial(request.NombreUsuario, usuarioExistente);
            await this._inicialLogica.IniciarPartida(usuario);
            
            var usuarioRegistrado = await this._authLogica.IniciarSesion(request.NombreUsuario);
            var token = this._authLogica.GenerarToken(usuarioRegistrado);
            
            _configurarCookieLogica.ConfigurarCookie(token, DateTimeOffset.UtcNow.AddDays(7));

            var dto = base.Mapear<UsuarioDTO>(usuarioRegistrado);
            return Ok(dto);
        }
        catch (DominioException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Problem("Ocurrió un error al realizar el registro.");
        }
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        try
        {
            Response.Cookies.Delete("jwt-auth");
            return Ok();
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al cerrar sesión");
        }
    }

    [HttpGet("Validar")]
    public async Task<IActionResult> Validar()
    {
        try
        {
            int idUsuario = this._accesoUsuarios.ObtenerIdUsuarioActual();
            var usuario = await this._usuarioLogica.ObtenerPorId(idUsuario);
            var dto = base.Mapear<UsuarioDTO>(usuario);
            return Ok(dto);
        }
        catch (DominioException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem("Ocurrió un error al validar el usuario");
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

            _configurarCookieLogica.ConfigurarCookie(token, DateTimeOffset.UtcNow.AddDays(7));

            var response = new LoginDTO
            {
                NombreUsuario = usuario.NombreUsuario!,
                Mail = usuario.Mail!,
                IdUsuario = usuario.Id,
                IdPartida = partida?.Id ?? 0
            };

            return Ok(response);
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

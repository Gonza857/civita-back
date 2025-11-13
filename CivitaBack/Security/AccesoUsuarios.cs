using CivitaBack.Domain.Excepciones;
using CivitaBack.Logica.Interfaces;
using System.Security.Claims;

namespace CivitaBack.Api.Security
{
    public class AccesoUsuarios : IAccesoUsuarios
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccesoUsuarios(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public bool EsDios()
        {
            var principal = ObtenerClaimsPrincipal();

            var esDiosClaim = principal?.FindFirst("EsDios");

            if (esDiosClaim == null) return false;

            return bool.TryParse(esDiosClaim.Value, out bool esDios) && esDios;
        }

        public int ObtenerIdUsuarioActual()
        {
            var principal = ObtenerClaimsPrincipal();

            var idUsuarioClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);

            if (idUsuarioClaim != null && int.TryParse(idUsuarioClaim.Value, out int idUsuario))
            {
                return idUsuario;
            }

            return 0;
        }

        public void ValidarAcceso(int idUsuarioPartida)
        {
            if (this.EsDios()) return;

            var usuarioActualId = this.ObtenerIdUsuarioActual();

            if (usuarioActualId != idUsuarioPartida)
                throw new AccesoDenegadoExcepcion("No tenes permiso para acceder a esta partida.");
        }

        private ClaimsPrincipal? ObtenerClaimsPrincipal()
        {
            // Obtiene el principal (Claims) del usuario autenticado de la solicitud actual
            return _httpContextAccessor.HttpContext?.User;
        }
    }
}

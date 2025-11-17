using CivitaBack.Logica.Interfaces;
using Microsoft.AspNetCore.Http;      
using Microsoft.Extensions.Hosting;  

namespace CivitaBack.Logica
{
    public class ConfigurarCookieLogica : IConfigurarCookieLogica
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _env;

        public ConfigurarCookieLogica(IHttpContextAccessor httpContextAccessor, IHostEnvironment env)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        public void ConfigurarCookie(string token, DateTimeOffset expires)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null) return;

            var isDevelopment = _env.IsDevelopment();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
                Secure = !isDevelopment,
                SameSite = isDevelopment ? SameSiteMode.Lax : SameSiteMode.Strict
            };

            httpContext.Response.Cookies.Append(
                "jwt-auth",
                token,
                cookieOptions
            );
        }
    }
}

namespace CivitaBack.Logica.Interfaces
{
    public interface IConfigurarCookieLogica
    {
        void ConfigurarCookie(string token, DateTimeOffset expires);
    }
}

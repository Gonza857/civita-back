namespace CivitaBack.Domain.Excepciones
{
    public class AutenticacionException : Exception
    {
        public AutenticacionException(string message) : base(message) { }
    }
}

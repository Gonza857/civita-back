namespace CivitaBack.Domain.Excepciones
{
    public class ValidacionRegistroException : Exception
    {
        public ValidacionRegistroException(string mensaje) : base(mensaje) { }
    }
}

namespace CivitaBack.Domain.Excepciones
{
    internal class TransaccionException : Exception
    {
        public TransaccionException(string mensaje) : base(mensaje)
        {
        }
    }
}

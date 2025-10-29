namespace CivitaBack.Domain.Excepciones;

public class PersistenciaException : Exception
{
    public PersistenciaException(string mensaje) : base(mensaje)
    {

    }
}
namespace CivitaBack.Logica.Excepciones;

public class PersistenciaException : Exception
{
    public PersistenciaException(string mensaje) : base(mensaje)
    {

    }
}
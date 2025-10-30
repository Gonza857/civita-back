namespace CivitaBack.Domain.Excepciones;

public class CondicionExcepcion : Exception
{
    public CondicionExcepcion(string mensaje) : base(mensaje)
    {
    }
}
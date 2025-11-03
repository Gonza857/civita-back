namespace CivitaBack.Domain.Excepciones;

public class MisionExcepcion : Exception
{
    public MisionExcepcion(string mensaje) : base(mensaje)
    {
        
    }
}
namespace CivitaBack.Domain.Excepciones;

public class EstructuraExcepcion : Exception
{
    public EstructuraExcepcion(string mensaje) : base(mensaje)
    {

    }
}
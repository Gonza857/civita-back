namespace CivitaBack.Domain.Excepciones;

public class ErrorInternoExcepction : Exception
{
    public ErrorInternoExcepction() { }

    public ErrorInternoExcepction(string message) : base(message) { }
}

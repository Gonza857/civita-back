namespace CivitaBack.Domain.Excepciones;

public class ErrorInternoException : Exception
{
    public ErrorInternoException(string message) : base(message) { }
}

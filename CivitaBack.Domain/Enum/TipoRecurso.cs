namespace CivitaBack.Domain.Enum;

public enum TipoRecurso
{
    Energia,
    Felicidad,
    Contaminacion,
    EcoCoins,
    Experiencia
}

public static class TipoRecursoHelper
{
    public static bool TryGetTipoRecurso(string input, out TipoRecurso tipoRecurso)
    {
        input = input?.Trim();
        return System.Enum.TryParse<TipoRecurso>(input, ignoreCase: true, out tipoRecurso);
    }

    public static bool EsTipoRecursoValido(string input)
    {
        input = input?.Trim();
        return System.Enum.GetNames(typeof(TipoRecurso))
            .Any(e => e.Equals(input, StringComparison.OrdinalIgnoreCase));
    }

    public static TipoRecurso? ParseTipoRecurso(string input)
    {
        input = input?.Trim();
        if (System.Enum.TryParse<TipoRecurso>(input, true, out var result))
            return result;
        return null;
    }

    public static IEnumerable<string> ObtenerTodosLosTipos()
    {
        return System.Enum.GetNames(typeof(TipoRecurso));
    }
}

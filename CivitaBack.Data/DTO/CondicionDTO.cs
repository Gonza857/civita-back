using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;

namespace CivitaBack.Tests;

public class EstructuraCondicionDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class CondicionDTO
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public string? NombreColumna { get; set; }  // recurso
    public int? EstructuraId { get; set; }      // solo si es estructura
    public EstructuraCondicionDTO? Estructura { get; set; }

    // Método helper para renderizar la condición como string
    public string Render()
    {
        if (!string.IsNullOrWhiteSpace(NombreColumna))
            return $"{NombreColumna} - {Cantidad}";
        if (Estructura != null)
            return $"{Estructura.Nombre} - {Cantidad}";
        return "Condición inválida";
    }
}
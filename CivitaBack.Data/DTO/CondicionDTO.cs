using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;

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
    public bool EsRecompensa { get; set; }
    public int? EstructuraId { get; set; }      // solo si es estructura
    public EstructuraCondicionDTO? Estructura { get; set; }

    // Propiedad que apunta a la recompensa asociada (si tiene)
    public int? RecompensaId { get; set; }
    public CondicionDTO? Recompensa { get; set; }

    // ✅ Propiedad calculada para mostrar en la tabla del front
    // public string RecompensaDescripcion => Recompensa != null 
    //     ? (!string.IsNullOrWhiteSpace(Recompensa.NombreColumna)
    //         ? $"{Recompensa.NombreColumna} - {Recompensa.Cantidad}"
    //         : $"{Recompensa.Estructura?.Nombre} - {Recompensa.Cantidad}")
    //     : string.Empty;
}

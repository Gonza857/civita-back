namespace CivitaBack.Data.DTO;

public class ActualizarCondicionDTO
{
    // Campos editables de la condición
    public int Cantidad { get; set; }
    public string? NombreColumna { get; set; }
    public int? EstructuraId { get; set; }

    // ✅ LA SOLUCIÓN: Una lista de IDs
    // "Quiero que esta condición entregue las recompensas 5, 8 y 12"
    public List<int> RecompensaIds { get; set; } = new List<int>();
}
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

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
    public List<RecompensaDTO> Recompensas { get; set; } = new List<RecompensaDTO>();
    
    public List<int> RecompensaIds { get; set; } = new List<int>();
}

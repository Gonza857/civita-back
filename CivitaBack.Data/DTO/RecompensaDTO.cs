namespace CivitaBack.Data.DTO;

public class RecompensaDTO
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public string? NombreColumna { get; set; }

    public int? EstructuraId { get; set; }
    public EstructuraDTO? Estructura { get; set; }
}
namespace CivitaBack.Data.BO;

public class RecompensaEF : AuditableEF
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public string? NombreColumna { get; set; }

    public int? EstructuraId { get; set; }
    public EstructuraEF? Estructura { get; set; }
    
    public virtual ICollection<CondicionEF> Condiciones { get; set; } = new List<CondicionEF>();
}
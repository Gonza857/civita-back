using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Condicion : Auditable
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public string? NombreColumna { get; set; }

    public int? EstructuraId { get; set; }
    public Estructura? Estructura { get; set; }
    
    public virtual ICollection<Recompensa> Recompensas { get; set; } = new List<Recompensa>();
}

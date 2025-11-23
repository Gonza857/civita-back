using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Recompensa: Auditable
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public string? NombreColumna { get; set; }

    public int? EstructuraId { get; set; }
    public Estructura? Estructura { get; set; }
    
    public virtual ICollection<Condicion> Condiciones { get; set; } = new List<Condicion>();
}
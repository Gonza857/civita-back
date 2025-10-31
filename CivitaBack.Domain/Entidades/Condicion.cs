using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Condicion : Auditable
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public string? NombreColumna { get; set; }

    public int? EstructuraId { get; set; }
    public Estructura? Estructura { get; set; }

    public bool EsRecompensa { get; set; } = false;

    // Self-reference
    public int? RecompensaId { get; set; }
    public Condicion? Recompensa { get; set; }
    public ICollection<Condicion>? CondicionesAsociadas { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO;

public class CondicionEF : AuditableEF
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public string? NombreColumna {get; set;}
    public int? EstructuraId { get; set; }
    public EstructuraEF? Estructura { get; set; }

    public bool EsRecompensa { get; set; } = false;
    public int? RecompensaId { get; set; }     // 👉 FK a otra condición
    public CondicionEF? Recompensa { get; set; } // 👉 navegación hacia esa "otra" condición

    public ICollection<CondicionEF>? CondicionesAsociadas { get; set; } // 👉 navegación inversa
}

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

    public virtual ICollection<RecompensaEF> Recompensas { get; set; } = new List<RecompensaEF>();

}

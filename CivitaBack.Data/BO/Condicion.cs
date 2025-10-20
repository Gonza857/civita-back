using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO;

public class Condicion : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public string? NombreColumna {get; set;}
    public int? EstructuraId { get; set; }
    public Estructura? Estructura { get; set; }
}

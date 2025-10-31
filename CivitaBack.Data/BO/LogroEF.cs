using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO;

public class LogroEF : AuditableEF
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Titulo { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public TipoLogroEF TipoLogro { get; set; }

    public CondicionEF Condicion { get; set; }
    public ICollection<LogroPartidaEF> LogroPartidas { get; set; }


}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.BO;

public class Logro
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Titulo { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public TipoLogro TipoLogro { get; set; }

    public Condicion Condicion { get; set; }
    public ICollection<LogroPartida> LogroPartidas { get; set; }


}

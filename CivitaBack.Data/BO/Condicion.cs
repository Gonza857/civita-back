using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.BO;

public class Condicion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Cantidad { get; set; }

    public int EstructuraId { get; set; }
    public Estructura Estructura { get; set; }

    public int RecursoId { get; set; }
    public Recurso Recurso { get; set; }
}

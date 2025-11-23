using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class TipEF : AuditableEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Mensaje { get; set; }

        public string? Expresion { get; set; }

        public string? ElementoAdicional { get; set; }
        public bool EfectoFiltro { get; set; }
        public int TipoId { get; set; }
        public TipoTipEF TipoTip { get; set; }
        
        public int? Orden { get; set; }

        public List<TipEnPartidaEF?> TipEnPartida { get; set; }

    }
}

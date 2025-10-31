using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class TipEnPartidaEF : AuditableEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Relaciones
        public int PartidaId { get; set; }
        public PartidaEF? Partida { get; set; }

        public int TipId { get; set; }
        public TipEF? Tip { get; set; }
    }
}

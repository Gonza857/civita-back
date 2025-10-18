using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class TipEnPartida : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Relaciones
        public int PartidaId { get; set; }
        public Partida? Partida { get; set; }

        public int TipId { get; set; }
        public Tip? Tip { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class Tip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Mensaje { get; set; }

        public int TipoTipId { get; set; }
        public TipoTip? TipoTip { get; set; }

        public List<TipEnPartida>? TipEnPartida { get; set; }
    }
}

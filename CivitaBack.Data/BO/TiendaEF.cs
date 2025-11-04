using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class TiendaEF : AuditableEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EstructuraId { get; set; }
        public EstructuraEF? Estructura { get; set; }

        public int PartidaId { get; set; }
        public PartidaEF? Partida { get; set; }
    }
}

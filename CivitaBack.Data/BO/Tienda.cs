using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class Tienda : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? NombreArticulo { get; set; }
        public string? CodigoArticulo { get; set; }

        public int PartidaId { get; set; }
        public PartidaEF? Partida { get; set; }
    }
}

using CivitaBack.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class EfectoEventoEF : AuditableEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventoMaestroId { get; set; }
        public EventoMaestroEF? EventoMaestro { get; set; }
        public TipoResultado TipoResultado { get; set; }
        public int EcoCoins { get; set; }
        public int Felicidad { get; set; }
        public int Contaminacion { get; set; }
        public int Energia { get; set; }
        public int Experiencia { get; set; }
    }
}

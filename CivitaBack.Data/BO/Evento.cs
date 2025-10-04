using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class Evento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? DescripcionEvento { get; set; }
        public int TiempoParaHacerlo { get; set; }
        
        //public string? Tipo { get; set; } 
      
        public int EventoMaestroId { get; set; }
        public EventoMaestro? EventoMaestro { get; set; }

        public int PartidaId { get; set; }
        public Partida? Partida { get; set; }
    }
}

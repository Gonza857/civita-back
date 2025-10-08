using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;

namespace CivitaBack.Data.BO
{
    public class Partida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? UltimaVez { get; set; }
        public string? JsonMapa { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Relaciones
        public List<Recurso>? Recurso { get; set; }
        public List<Evento>? Evento { get; set; }
        public List<Tienda>? Tienda { get; set; }
        public List<EstructuraEnMapa>? EstructuraEnMapa { get; set; }
        public List<TipEnPartida>? TipEnPartida { get; set; }
        // Relación N:N
        public ICollection<LogroPartida> LogroPartidas { get; set; }
    }
}

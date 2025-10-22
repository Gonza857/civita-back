using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CivitaBack.Data.BO
{
    public class Partida : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? UltimaVez { get; set; }
        public string? JsonMapa { get; set; }

        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        // Relaciones
        [JsonIgnore]
        public Recurso? Recursos { get; set; }
        public List<Evento>? Evento { get; set; }
        public List<Tienda>? Tienda { get; set; }
        public List<EstructuraMapa>? EstructuraMapa { get; set; }
        public List<TipEnPartida>? TipEnPartida { get; set; }
        // Relación N:N
        public List<LogroPartida> LogroPartidas { get; set; }
    }
}

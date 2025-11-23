using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CivitaBack.Data.BO
{
    public class PartidaEF : AuditableEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? UltimaVez { get; set; }
        public string? JsonMapa { get; set; }

        public int Nivel { get; set; }
        public int Experiencia { get; set; }
        public int UsuarioId { get; set; }
        public bool EstaPausada { get; set; }

        [JsonIgnore]
        public UsuarioEF? Usuario { get; set; }

        // Relaciones
        public RecursoEF? Recursos { get; set; }
        public List<EventoEF>? Evento { get; set; }
        public List<TiendaEF>? Tienda { get; set; }
        public List<EstructuraMapaEF> EstructuraMapa { get; set; } = new List<EstructuraMapaEF>();
        public List<TipEnPartidaEF>? TipEnPartida { get; set; }
        // Relación N:N
        public List<LogroPartidaEF> LogroPartidas { get; set; }
        public List<MisionPartidaEF> MisionPartidas { get; set; }
    }
}

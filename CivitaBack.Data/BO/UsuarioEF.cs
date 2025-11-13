using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CivitaBack.Data.BO
{
    public class UsuarioEF : AuditableEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? NombreUsuario { get; set; }
        public string? Mail { get; set; }
        public string? HashDeContrasena { get; set; }

        public bool EsDios { get; set; } = false;

        // Relaciones
        [JsonIgnore]
        public PartidaEF Partida { get; set; }
    }
}

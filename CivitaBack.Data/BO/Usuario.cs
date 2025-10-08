using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CivitaBack.Data.BO
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? NombreUsuario { get; set; }
        public string? Mail { get; set; }
        public string? HashDeContrasena { get; set; }

        // Relaciones
        [JsonIgnore]
        public Partida Partida { get; set; }
    }
}

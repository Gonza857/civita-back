using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CivitaBack.Data.BO;

public class EstructuraMapa : Auditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Relaciones
    public int PartidaId { get; set; }
    [JsonIgnore]
    public Partida? Partida { get; set; }
    public int EstructuraId { get; set; }
    [JsonIgnore]
    public Estructura? Estructura { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

}

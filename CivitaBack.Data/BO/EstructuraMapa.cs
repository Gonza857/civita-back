using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CivitaBack.Data.BO;

public class EstructuraMapaEF : AuditableEF
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Relaciones
    public int PartidaId { get; set; }
    [JsonIgnore]
    public PartidaEF? Partida { get; set; }
    public int EstructuraId { get; set; }
    [JsonIgnore]
    public EstructuraEF? Estructura { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

}

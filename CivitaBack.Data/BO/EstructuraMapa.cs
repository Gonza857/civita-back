using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO;

public class EstructuraMapa
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Relaciones
    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }

    public int EstructuraId { get; set; }
    public Estructura? Estructura { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

}

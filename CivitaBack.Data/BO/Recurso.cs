using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO;

public class Recurso
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? Nombre { get; set; }
    public int Cantidad { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }

    // Constructor
    public Recurso(string nombre, int cantidad, Partida partida)
    {
        Nombre = nombre;
        Cantidad = cantidad;
        Partida = partida;
        PartidaId = partida.Id;
    }

    // Constructor vacío requerido por EF
    public Recurso() { }
}

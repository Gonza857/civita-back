using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CivitaBack.Data.BO;

public class RecursoEF : AuditableEF
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Energia { get; set; }
    public int Contaminacion { get; set; }
    public int Felicidad { get; set; }
    public int EcoCoins { get; set; }
    public int Poblacion { get; set; }
    public int PartidaId { get; set; }
    [JsonIgnore]
    public PartidaEF? Partida { get; set; }

    public RecursoEF() { }

    public RecursoEF(
        int energia, 
        int contaminacion, 
        int felicidad, 
        int ecoCoins, 
        PartidaEF partida)
    {
        Energia = energia;
        Contaminacion = contaminacion;
        Felicidad = felicidad;
        EcoCoins = ecoCoins;
        PartidaId = partida.Id;
    }

    
}

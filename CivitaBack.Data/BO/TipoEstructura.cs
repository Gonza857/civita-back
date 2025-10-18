using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class TipoEstructura : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Nombre { get; set; }  // Ej: Generadora, Vivienda, Verde
        public string? Ocupacion { get; set; }
        public int Capacidad { get; set; }
        public int EnergiaPorCiclo { get; set; }
        public int DineroPorCiclo { get; set; }

        public List<Estructura>? Estructura { get; set; }
    }
}

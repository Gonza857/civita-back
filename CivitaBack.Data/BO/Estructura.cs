using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class Estructura : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Nombre { get; set; }
        public bool EsMejorable { get; set; }
        public string? RutaImagen { get; set; }
        public int CostoEnergia { get; set; }
        public int CostoDinero { get; set; }
        public int FelicidadCiclo { get; set; }
        public int ContaminacionCiclo { get; set; }
        public int TipoEstructuraId { get; set; }
        public TipoEstructura TipoEstructura { get; set; }
        public List<EstructuraMapa>? EstructurasEnMapa { get; set; }
    }
}

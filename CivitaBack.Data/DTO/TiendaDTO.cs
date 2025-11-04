
namespace CivitaBack.Data.DTO
{
    public class TiendaDTO
    {
        public int EstructuraId { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public string RutaImagen { get; set; } = string.Empty;
        public int CostoDinero { get; set; }
        public int TipoEstructuraId { get; set; }
        public string TipoNombre { get; set; } = string.Empty;

        public int FelicidadCiclo { get; set; }    
        public int ContaminacionCiclo { get; set; }
        public int DineroCiclo { get; set; }
        public int EnergiaCiclo { get; set; }
    }
}

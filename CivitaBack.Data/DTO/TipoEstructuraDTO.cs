namespace CivitaBack.Data.DTO;

public class TipoEstructuraDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Ocupacion { get; set; }
    public int Capacidad { get; set; }
    public int EnergiaPorCiclo { get; set; }
    public int DineroPorCiclo { get; set; }
}
using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class TipoEstructura : Auditable
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Ocupacion { get; set; }
    public int Capacidad { get; set; }
    public int EnergiaPorCiclo { get; set; }
    public int DineroPorCiclo { get; set; }

    public List<Estructura>? Estructura { get; set; }
}

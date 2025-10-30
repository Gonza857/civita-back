using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entities;

public class EventoMaestro : Auditable
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string TextoDescripcion { get; set; } = string.Empty;
    public string TextoAceptar { get; set; } = string.Empty;
    public string TextoRechazar { get; set; } = string.Empty;
    public int EcoCoinsAceptar { get; set; }
    public int FelicidadAceptar { get; set; }
    public int ContaminacionAceptar { get; set; }
    public int FelicidadRechazar { get; set; }
    public int ContaminacionRechazar { get; set; }

    public List<Evento>? Evento { get; set; }
}

using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Evento : Auditable
{
    public int Id { get; set; }

    public string TextoDescripcion { get; set; } = string.Empty;
    public string TextoAceptar { get; set; } = string.Empty;
    public string TextoRechazar { get; set; } = string.Empty;

    public int EcoCoinsAceptar { get; set; }
    public int FelicidadAceptar { get; set; }
    public int ContaminacionAceptar { get; set; }
    public int FelicidadRechazar { get; set; }
    public int ContaminacionRechazar { get; set; }

    public bool SeDisparo { get; set; } = false;
    public bool Resuelto { get; set; } = false;

    public int EventoMaestroId { get; set; }
    public EventoMaestro? EventoMaestro { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }
}


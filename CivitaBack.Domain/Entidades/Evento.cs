using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Evento : Auditable
{
    public int Id { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public string? RespuestaJugador { get; set; }
    public int EcoCoinsAplicada { get; set; }
    public int FelicidadAplicada { get; set; }
    public int ContaminacionAplicada { get; set; }
    public int EnergiaAplicada { get; set; }
    public int ExperienciaAplicada { get; set; }
    public bool SeDisparo { get; set; } = false;
    public bool Resuelto { get; set; } = false;
    public int EventoMaestroId { get; set; }
    public int PartidaId { get; set; }
    public EventoMaestro? EventoMaestro { get; set; }
    public Partida? Partida { get; set; }
}


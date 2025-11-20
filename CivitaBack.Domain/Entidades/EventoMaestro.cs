using CivitaBack.Domain.Common;
using CivitaBack.Domain.Enum;

namespace CivitaBack.Domain.Entidades;

public class EventoMaestro : Auditable
{
    public int Id { get; set; }
    public TipoEvento TipoEvento { get; set; } = TipoEvento.PREGUNTA;
    public string Titulo { get; set; } = string.Empty;
    public string ContenidoPrincipal { get; set; } = string.Empty;
    public string OpcionA_Texto { get; set; } = string.Empty;
    public string OpcionB_Texto { get; set; } = string.Empty;
    public string RespuestaCorrecta { get; set; } = "A";
    public List<EfectoEvento>? Efectos { get; set; }
    public List<Evento>? Evento { get; set; }
}

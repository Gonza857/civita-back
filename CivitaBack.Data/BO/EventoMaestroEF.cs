using CivitaBack.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CivitaBack.Data.BO
{
    public class EventoMaestroEF : AuditableEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TipoEvento TipoEvento { get; set; } = TipoEvento.PREGUNTA;
        public string Titulo { get; set; } = string.Empty;
        public string ContenidoPrincipal { get; set; } = string.Empty; 
        public string OpcionA_Texto { get; set; } = string.Empty;
        public string OpcionB_Texto { get; set; } = string.Empty;
        public string RespuestaCorrecta { get; set; } = "A";
        public List<EfectoEventoEF>? Efectos { get; set; }
        public List<EventoEF>? Evento { get; set; }
    }
}

using System.ComponentModel;

namespace CivitaBack.Domain.Enum;

public enum TipoEvento
{
    [Description("Evento con pregunta de opción múltiple")]
    PREGUNTA = 1,

    [Description("Tip o Notificación Informativa")]
    TIP_INFORMATIVO = 2,

    [Description("Evento con decisión binaria (Aceptar/Rechazar)")]
    DECISION_BINARIA = 3,

    
}

using CivitaBack.Domain.Enum;

namespace CivitaBack.Data.BO;

public class MisionEF : AuditableEF
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public bool Disponible { get; set; }
    public int CondicionId { get; set; }
    public CondicionEF Condicion { get; set; }
    
    public TipoMision Tipo { get; set; }
}
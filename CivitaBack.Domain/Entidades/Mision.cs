using CivitaBack.Domain.Common;
using CivitaBack.Domain.Enum;

namespace CivitaBack.Domain.Entidades;

public class Mision : Auditable
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public bool Disponible { get; set; }

    // Reutiliza EXACTAMENTE el mismo sistema
    public int CondicionId { get; set; }
    public Condicion Condicion { get; set; }
    
    public TipoMision Tipo { get; set; }
}
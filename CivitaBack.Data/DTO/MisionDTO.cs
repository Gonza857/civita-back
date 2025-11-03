using CivitaBack.Domain.Entidades;

namespace CivitaBack.Data.DTO;

public class MisionDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public bool Disponible { get; set; }
    public string Tipo { get; set; }
    public int CondicionId { get; set; }
    public CondicionDTO? Condicion { get; set; }
}
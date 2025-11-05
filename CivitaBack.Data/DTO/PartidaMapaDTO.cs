using CivitaBack.Domain.Entidades;

namespace CivitaBack.Data.DTO;

public class PartidaMapaDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime UltimaVez { get; set; }
    public string JsonMapa {get; set;}
    public List<EstructuraMapaDTO>? estructuras {get; set;}
}
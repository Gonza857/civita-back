using CivitaBack.Domain.Entidades;

namespace CivitaBack.Data.DTO;

public class PartidaMapaDTO
{
    int Id { get; set; }
    int UsuarioId { get; set; }
    DateTime UltimaVez { get; set; }
    string JsonMapa {get; set;}
    List<EstructuraMapa> estructuras {get; set;}
}
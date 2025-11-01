using System.ComponentModel.DataAnnotations;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Data.DTO;

public class PartidaDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }

    public string? Usuario { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La energía no puede ser negativa")]
    public int Energia {  get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La felicidad no puede ser negativa")]
    public int Felicidad { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La contaminación no puede ser negativa")]
    public int Contaminacion { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Las EcoCoins no pueden ser negativas")]
    public int EcoCoins { get; set; }
    public Partida? Partida { get; set; } 

}

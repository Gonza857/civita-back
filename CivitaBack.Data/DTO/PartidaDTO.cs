using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;

namespace CivitaBack.Data.DTO;

public class PartidaDTO
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }

    public string? Usuario { get; set; }

    public int Energia {  get; set; }
    public int Felicidad { get; set; }
    public int Contaminacion { get; set; }
    public int EcoCoins { get; set; }
    public Partida? Partida { get; set; } 

}

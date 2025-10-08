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
    public Partida Partida { get; set; } // Ya trae la partida

}

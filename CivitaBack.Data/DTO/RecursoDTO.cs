using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.DTO;

public class RecursoDTO
{ 
    public int Energia { get; set; }
    public int Contaminacion { get; set; }
    public int Felicidad { get; set; }
    public int EcoCoins { get; set; }
    public int Poblacion { get; set; }
    
    public int? Nivel { get; set; }
    public int? Experiencia { get; set; }
    public int? ExperienciaSiguienteNivel { get; set; }

}

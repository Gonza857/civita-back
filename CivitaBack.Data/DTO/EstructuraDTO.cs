using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.DTO;

public class EstructuraDTO
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public bool EsMejorable { get; set; }
    public string? RutaImagen { get; set; }
    public int CostoEnergia { get; set; }
    public int CostoDinero { get; set; }
    public int FelicidadCiclo { get; set; }
    public int ContaminacionCiclo { get; set; }
    public int TipoEstructuraId { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Tests;

namespace CivitaBack.Data.DTO;

public class LogroDTO
{   
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int CondicionId { get; set; }
    public CondicionDTO Condicion { get; set; } = new CondicionDTO();
    public int TipoId { get; set; }
    public string? Tipo { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;

namespace CivitaBack.Data.DTO;

public class LogroDTO
{   
    public int Id { get; set; }

    public string Titulo { get; set; }

    public string Descripcion { get; set; }

    public int TipoId { get; set; }

    public string? Tipo { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Data.BO;

public class TestModelado
{
    [Key]
    public int Id { get; set; } = 0;
    public string? Correo { get; set; }
}

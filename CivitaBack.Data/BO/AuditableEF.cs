using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.BO;

[Owned]
public class AuditableEF
{
    public DateTime Creado { get; set; } = DateTime.UtcNow;
    public DateTime? Editado { get; set; }

}

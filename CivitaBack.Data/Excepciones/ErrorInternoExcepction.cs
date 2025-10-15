using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Logica.Excepciones;

public class ErrorInternoExcepction : Exception
{
    public ErrorInternoExcepction() { }

    public ErrorInternoExcepction(string message) : base(message) { }
}

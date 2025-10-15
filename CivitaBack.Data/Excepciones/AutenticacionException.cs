using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Logica.Excepciones
{
    public class AutenticacionException : Exception
    {
        public AutenticacionException(string message) : base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Domain.Excepciones
{
    public class AccesoDenegadoExcepcion : Exception
    {
        public AccesoDenegadoExcepcion(string message) : base(message)
        {
        }
    }
}

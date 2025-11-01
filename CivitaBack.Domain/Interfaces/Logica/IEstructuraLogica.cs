using CivitaBack.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface IEstructuraLogica
    {
        Task<Estructura> ObtenerPorId(int idEstructura);
        Task<List<Estructura>> ObtenerListado();
        Task Crear(Estructura estructura);
        Task Eliminar(int idEstructura);
        Task Actualizar(Estructura estructura, int id);
    }
}

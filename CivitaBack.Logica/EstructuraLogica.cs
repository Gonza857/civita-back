using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface IEstructuraLogica
{
    Estructura ObtenerPorId(int idEstructura);
}
public class EstructuraLogica : IEstructuraLogica, IParser<Estructura, EstructuraDTO>
{
    private readonly IEstructuraRepositorio repositorioEstructura;

    public EstructuraLogica(IEstructuraRepositorio re)
    {
        repositorioEstructura = re;
    }

    public Estructura ObtenerPorId(int idEstructura)
    {
        if (idEstructura == null) throw new Exception("No se pudo obtener la estructura"); // Pasar a personalizada
        return this.repositorioEstructura.ObtenerPorId(idEstructura);
    }

    public EstructuraDTO ToDto(Estructura entidad)
    {
        return new EstructuraDTO
        {
            ContaminacionCiclo
            = entidad.ContaminacionCiclo,
            CostoDinero = entidad.CostoDinero,
            CostoEnergia = entidad.CostoEnergia,
            EsMejorable = entidad.EsMejorable,
            FelicidadCiclo = entidad.FelicidadCiclo,
            Id = entidad.Id,
            Nombre = entidad.Nombre,
            RutaImagen = entidad.RutaImagen,
            TipoEstructuraId = entidad.TipoEstructura.Id
        };
    }
}

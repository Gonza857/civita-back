using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface IEstructuraLogica
{
    Task<EstructuraDTO> ObtenerPorId(int idEstructura);
    Task<List<EstructuraDTO>> ObtenerListado();
    Task Crear(EstructuraDTO estructura);
    Task Eliminar (int idEstructura);
    Task Actualizar (EstructuraDTO estructura, int id);
}
public class EstructuraLogica : IEstructuraLogica, IParser<Estructura, EstructuraDTO>
{
    private readonly IEstructuraRepositorio _repositorioEstructura;
    private readonly ITipoEstructuraRepositorio _repositorioTipoEstructura;

    public EstructuraLogica(IEstructuraRepositorio re, ITipoEstructuraRepositorio ter)
    {
        _repositorioEstructura = re;
        _repositorioTipoEstructura = ter;
    }

    public async Task<EstructuraDTO> ObtenerPorId(int idEstructura)
    {
        if (idEstructura <= 0) throw new EstructuraExcepcion("No se pudo obtener la Estructura"); 
        Estructura? estructura = await this._repositorioEstructura.ObtenerPorId(idEstructura);
        if (estructura == null) return null;
        return this.ToDto(estructura);
    }

    public async Task<List<EstructuraDTO>> ObtenerListado()
    {
        var estructuras = await this._repositorioEstructura.ObtenerTodos();
        return estructuras
            .Select(p => this.ToDto(p))
            .ToList();
    }

    public async Task Crear(EstructuraDTO estructura)
    {
        this.Validar(estructura);
        TipoEstructura? tipoEstructura = await this._repositorioTipoEstructura.ObtenerPorId(estructura.Tipo.Id);
        if (tipoEstructura == null) 
            throw new EstructuraExcepcion("No se pudo crear la Estructura");

        Estructura estructuraNueva = new Estructura
        {
            ContaminacionCiclo = estructura.ContaminacionCiclo,
            CostoDinero = estructura.CostoDinero,
            CostoEnergia = estructura.CostoEnergia,
            EsMejorable = estructura.EsMejorable,
            RutaImagen = estructura.RutaImagen,
            TipoEstructura = tipoEstructura,
            FelicidadCiclo = estructura.FelicidadCiclo,
            Nombre = estructura.Nombre,
            Creado = DateTime.UtcNow,
        };
        
        await this._repositorioEstructura.Agregar(estructuraNueva);
    }

    public async Task Eliminar(int idEstructura)
    {
        if (idEstructura <= 0) 
            throw new EstructuraExcepcion("No se pudo borrar la Estructura");
        await this._repositorioEstructura.Eliminar(idEstructura);
    }

    public async Task Actualizar(EstructuraDTO estructura, int id)
    {
        this.Validar(estructura);
        TipoEstructura? tipoEstructuraBuscada = await this._repositorioTipoEstructura.ObtenerPorId(estructura.Tipo.Id);
        Estructura? estructuraBuscada = await this._repositorioEstructura.ObtenerPorId(id);
        
        if (tipoEstructuraBuscada == null || estructuraBuscada == null) 
            throw new LogroExcepcion("Ocurrió un error al actualizar la Estructura");

        estructuraBuscada.TipoEstructura = tipoEstructuraBuscada;
        estructuraBuscada.EsMejorable = estructura.EsMejorable;
        estructuraBuscada.CostoEnergia = estructura.CostoEnergia;
        estructuraBuscada.ContaminacionCiclo = estructura.ContaminacionCiclo;
        estructuraBuscada.FelicidadCiclo = estructura.FelicidadCiclo;
        estructuraBuscada.CostoDinero = estructura.CostoDinero;
        estructuraBuscada.RutaImagen = estructura.RutaImagen;
        estructuraBuscada.Nombre = estructura.Nombre;

        await this._repositorioEstructura.Actualizar(estructuraBuscada);
    }

    public EstructuraDTO ToDto(Estructura entidad)
    {
        return new EstructuraDTO
        {
            ContaminacionCiclo = entidad.ContaminacionCiclo,
            CostoDinero = entidad.CostoDinero,
            CostoEnergia = entidad.CostoEnergia,
            EsMejorable = entidad.EsMejorable,
            FelicidadCiclo = entidad.FelicidadCiclo,
            Id = entidad.Id,
            Nombre = entidad.Nombre,
            RutaImagen = entidad.RutaImagen,
            Tipo = new TipoEstructuraDTO
            {
                Id = entidad.TipoEstructura.Id,
                Nombre = entidad.TipoEstructura.Nombre,
            }
        };
    }
    
    /// <summary>
    /// Valida los datos entrantes del DTO
    /// </summary>
    /// <param name="estructuraDTO">EstructuraDTO</param>
    private void Validar(EstructuraDTO estructuraDTO)
    {
        if (estructuraDTO == null) 
            throw new EstructuraExcepcion("Ocurrió un error al actualizar la estructura");
    }
}

using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class EstructuraLogica : IEstructuraLogica
{
    private readonly IEstructuraRepositorio _repositorioEstructura;
    private readonly ITipoEstructuraRepositorio _repositorioTipoEstructura;
    private readonly IUnidadDeTrabajo _uow;

    public EstructuraLogica(IEstructuraRepositorio re, ITipoEstructuraRepositorio ter, IUnidadDeTrabajo uow)
    {
        _repositorioEstructura = re;
        _repositorioTipoEstructura = ter;
        _uow = uow;
    }

    public async Task<Estructura> ObtenerPorId(int idEstructura)
    {
        if (idEstructura <= 0) throw new EstructuraExcepcion("No se pudo obtener la Estructura"); 
        Estructura? estructura = await this._repositorioEstructura.ObtenerPorId(idEstructura);
        if (estructura == null) return null;
        return estructura;
    }

    public async Task<List<Estructura>> ObtenerListado()
    {
        var estructuras = await this._repositorioEstructura.ObtenerTodos();
        return estructuras;
    }

    public async Task Crear(Estructura estructura)
    {
        this.Validar(estructura);
        TipoEstructura? tipoEstructura = await this._repositorioTipoEstructura.ObtenerPorId(estructura.TipoEstructura.Id);
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

        await this._uow.CommitAsync();
    }

    public async Task Eliminar(int idEstructura)
    {
        if (idEstructura <= 0) 
            throw new EstructuraExcepcion("No se pudo borrar la Estructura");
        await this._repositorioEstructura.Eliminar(idEstructura);

        await this._uow.CommitAsync();
    }

    public async Task Actualizar(Estructura estructura, int id)
    {
        this.Validar(estructura);
        TipoEstructura? tipoEstructuraBuscada = await this._repositorioTipoEstructura.ObtenerPorId(estructura.TipoEstructura.Id);
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

        await this._uow.CommitAsync();
    }

    /// <summary>
    /// Valida los datos entrantes del DTO
    /// </summary>
    /// <param name="estructuraDTO">EstructuraDTO</param>
    private void Validar(Estructura estructura)
    {
        if (estructura == null) 
            throw new EstructuraExcepcion("Ocurrió un error al actualizar la estructura");
    }
}

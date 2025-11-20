using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class TipoLogroLogica : ITipoLogroLogica
{
    private readonly ITipoLogroRepositorio repositorioTipoLogro;
    private readonly IUnidadDeTrabajo _uow;
    private readonly IAccesoUsuarios _accesoUsuarios;

    public TipoLogroLogica(ITipoLogroRepositorio rtl, IUnidadDeTrabajo uow, IAccesoUsuarios accesoUsuarios)
    {
        repositorioTipoLogro = rtl;
        _uow = uow;
        _accesoUsuarios = accesoUsuarios;
    }

    private void ValidarTipoLogro(TipoLogro tipoLogro)
    {
        if (tipoLogro == null) 
            throw new TipoLogroException("Ocurrió un error al actualizar el Tipo de Logro");
    }

    /// <summary>
    /// Actualiza un Tipo de Logro
    /// </summary>
    /// <param name="tipoLogro">TipoLogro</param>
    /// <param name="idTipoLogro">Id de Tipo Logro</param>
    public async Task Actualizar(TipoLogro tipoLogro, int idTipoLogro)
    {
        this.ValidarTipoLogro(tipoLogro);
        var tipoLogroBuscado = await this.repositorioTipoLogro.ObtenerPorId(idTipoLogro);
        if (tipoLogroBuscado == null)
            throw new TipoLogroException("Tipo de Logro no encontrado");

        ValidarAdmin();

        tipoLogroBuscado.Nombre = tipoLogro.Nombre;
        await this.repositorioTipoLogro.Actualizar(tipoLogroBuscado);

        await _uow.CommitAsync();
    }

    /// <summary>
    /// Elimina un Tipo de logro
    /// </summary>
    /// <param name="id">Id de Tipo Logro</param>
    public async Task Eliminar(int id)
    {
        if (id <= 0) 
            throw new TipoLogroException("No se pudo borrar el Tipo de Logro");

        ValidarAdmin();

        await this.repositorioTipoLogro.Eliminar(id);

        await _uow.CommitAsync();
    }

    /// <summary>
    /// Guarda un Tipo de logro
    /// </summary>
    /// <param name="tipoLogro">tipoLogro</param>
    public async Task<TipoLogro> Guardar(TipoLogro tipoLogro)
    {
        this.ValidarTipoLogro(tipoLogro);

        ValidarAdmin();

        var tl = new TipoLogro
        {
            Nombre = tipoLogro.Nombre
        }; 
        
        await this.repositorioTipoLogro.Agregar(tl);
        await _uow.CommitAsync();
        return tl;
    }

    /// <summary>
    /// Obtiene un Tipo de Logro por Id
    /// </summary>
    /// <param name="id">Id de Tipo Logro</param>
    public async Task<TipoLogro> ObtenerPorId(int id)
    {
        var tipoLogro = await this.repositorioTipoLogro.ObtenerPorId(id);
        if (tipoLogro == null) 
            throw new TipoLogroException("No se pudo encontrar el Tipo de Logro");
        return tipoLogro;
    }
    
    /// <summary>
    /// Obtiene listado de Tipos de Logro
    /// </summary>
    public async Task<List<TipoLogro>> ObtenerTiposLogro()
    {
        return await this.repositorioTipoLogro.ObtenerTodos();
    }

    private void ValidarAdmin()
    {
        if (!_accesoUsuarios.EsDios())
            throw new AccesoDenegadoExcepcion("Se requieren privilegios de administrador para modificar el catálogo de estructuras.");
    }

}

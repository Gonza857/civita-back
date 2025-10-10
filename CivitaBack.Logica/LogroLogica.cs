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

public interface ILogroLogica
{
    LogroDTO ObtenerPorId(int Id);
    LogroDTO Guardar(LogroDTO entidad);

    List<LogroDTO> ObtenerListado();

    void Eliminar(int Id);
}

public class LogroLogica : IParser<Logro, LogroDTO>, ILogroLogica
{
    private readonly ILogroRepositorio repositorioLogro;
    private readonly ITipoLogroRepositorio repositorioTipoLogro;

    public LogroLogica(ILogroRepositorio rtl, ITipoLogroRepositorio itlr)
    {
        repositorioLogro = rtl;
        repositorioTipoLogro = itlr;
    }

    public void Eliminar(int Id)
    {
        if (Id == null) throw new Exception("No se pudo borrar el Logro");
        this.repositorioLogro.Eliminar(Id);
    }

    public LogroDTO Guardar(LogroDTO entidad)
    {
        if (entidad == null) throw new Exception("No se pudo guardar el Logro");
        TipoLogro tipoLogro = this.repositorioTipoLogro.ObtenerPorId(entidad.TipoId);
        if (tipoLogro == null) throw new Exception("No se pudo guardar el Logro");

        var logro = new Logro
        {
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            TipoLogro = tipoLogro
            
        };
        this.repositorioLogro.Guardar(logro);
        return this.ToDto(logro);
    }

    public List<LogroDTO> ObtenerListado()
    {
        var logros = this.repositorioLogro.ObtenerTodos();
        return logros
          .Select(p => this.ToDto(p))
          .ToList();
    }

    public LogroDTO ObtenerPorId(int Id)
    {
        var logro = this.repositorioLogro.ObtenerPorId(Id);
        if (logro == null) return null;
        return this.ToDto(logro);
    }

    public LogroDTO ToDto(Logro entidad)
    {
        return new LogroDTO
        {
            Id = entidad.Id,
            Titulo = entidad.Titulo,
            Descripcion = entidad.Descripcion,
            TipoId =  entidad.TipoLogro.Id,
            Tipo = entidad.TipoLogro.Nombre
        };
    }
}

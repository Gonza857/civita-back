using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;

namespace CivitaBack.Logica;

public interface ITipoLogroLogica
{
    TipoLogroDTO ObtenerPorId(int Id);
    TipoLogroDTO Guardar(TipoLogroDTO nuevoTipologro);

    List<TipoLogroDTO> ObtenerTiposLogro();

    void Eliminar(int Id);
}
public class TipoLogroLogica : ITipoLogroLogica
{
    private readonly ITipoLogroRepositorio repositorioTipoLogro;

    public TipoLogroLogica(ITipoLogroRepositorio rtl)
    {
        repositorioTipoLogro = rtl;
    }

    public void Eliminar(int Id)
    {
        if (Id == null) throw new Exception("No se pudo borrar el Tipo de Logro");
        this.repositorioTipoLogro.Eliminar(Id);
    }

    public TipoLogroDTO Guardar(TipoLogroDTO nuevoTipologro)
    {
        var tipoLogro = new TipoLogro
        {
            Nombre = nuevoTipologro.Nombre
        };
        this.repositorioTipoLogro.Guardar(tipoLogro);
        return this.TipoLogroToDTO(tipoLogro);
    }

    public TipoLogroDTO ObtenerPorId(int Id)
    {
        var TipoLogro = this.repositorioTipoLogro.ObtenerPorId(Id);
        if (TipoLogro == null) return null;
        return this.TipoLogroToDTO(TipoLogro);
    }

    public List<TipoLogroDTO> ObtenerTiposLogro()
    {
        var TiposLogros = this.repositorioTipoLogro.ObtenerTodos();
        return TiposLogros
          .Select(p => this.TipoLogroToDTO(p))
          .ToList();
    }

    private TipoLogroDTO TipoLogroToDTO(TipoLogro t)
    {
        return new TipoLogroDTO
        {
            Id = t.Id,
            Nombre = t.Nombre,
        };
    }
}

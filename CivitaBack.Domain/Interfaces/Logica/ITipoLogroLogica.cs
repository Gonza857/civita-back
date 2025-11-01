using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface ITipoLogroLogica
{
    Task<TipoLogro> ObtenerPorId(int Id);
    Task<TipoLogro> Guardar(TipoLogro nuevoTipologro);

    Task Actualizar(TipoLogro tipoLogro, int idTipoLogro);

    Task<List<TipoLogro>> ObtenerTiposLogro();

    Task Eliminar(int id);
}
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface ITipoEstructuraLogica
{
    Task<TipoEstructura> ObtenerPorId(int id);
    Task<TipoEstructura> Crear(TipoEstructura nuevoTipologro);

    Task Actualizar(TipoEstructura tipoLogro, int idTipoLogro);

    Task<List<TipoEstructura>> Listado();

    Task Eliminar(int id);
}
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface ITipoTipLogica
{
    Task<List<TipoTip>> Listado();
    Task Eliminar(int id);
    Task Guardar(TipoTip tipoTipDto);
    Task<TipoTip> ObtenerPorId(int id);
    Task Actualizar(TipoTip tipoTipDto, int id);
}
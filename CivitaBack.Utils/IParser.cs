namespace CivitaBack.Utils;

public interface IParser<TEntidad, TDto>
{
    TDto ToDto(TEntidad entidad);
    //TEntidad ToEntity(TDto dto);
}
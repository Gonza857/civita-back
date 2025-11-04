using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class EstructuraProfile : Profile
{
    public EstructuraProfile()
    {
        // 1. Mapa de EF -> Dominio (para leer de la base de datos)
        CreateMap<EstructuraEF, Estructura>();

        // 2. Mapa de Dominio -> EF (para guardar en la base de datos)
        CreateMap<Estructura, EstructuraEF>();
        
        // 3. Mapa de Dominio -> DTO (para guardar devolver al frontend)
        CreateMap<Estructura, EstructuraDTO>()
            .ForMember(
                dto => dto.Tipo, // Para la propiedad "Tipo" del DTO...
                opt => opt.MapFrom(dominio => dominio.TipoEstructura) // ...usá el valor de "TipoEstructura" del Dominio.
            );
        
        CreateMap<EstructuraDTO, Estructura>()
            .ForMember(
                dominio => dominio.TipoEstructuraId,
                opt => opt.MapFrom(dto => dto.TipoId)
            );;
        
        CreateMap<Estructura, EstructuraCondicionDTO>();
    }
}
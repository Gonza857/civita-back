using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades; 

namespace CivitaBack.Api.Mapping;

public class TipoEstructuraProfile : Profile
{
    public TipoEstructuraProfile()
    {
        // 1. Mapa de EF -> Dominio (para leer de la base de datos)
        CreateMap<TipoEstructuraEF, TipoEstructura>();

        // 2. Mapa de Dominio -> EF (para guardar en la base de datos)
        CreateMap<TipoEstructura, TipoEstructuraEF>()
            .ForMember(
                dto => dto.Estructura, // Asumo que la propiedad en tu DTO se llama 'Estructuras'
                opt => opt.Ignore() // ¡AQUÍ ESTÁ LA SOLUCIÓN!
            );;
        
        // 3. Mapa de Dominio -> DTO (para devolver al frontend)
        CreateMap<TipoEstructura, TipoEstructuraDTO>();
        
        CreateMap<TipoEstructuraDTO, TipoEstructura>();

    }
}
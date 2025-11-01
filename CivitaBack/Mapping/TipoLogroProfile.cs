using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class TipoLogroProfile : Profile
{
    public TipoLogroProfile()
    {
        // 1. Mapa de EF -> Dominio (para leer de la base de datos)
        CreateMap<TipoLogroEF, TipoLogro>();

        // 2. Mapa de Dominio -> EF (para guardar en la base de datos)
        CreateMap<TipoLogro, TipoLogroEF>();
        
        // 3. Mapa de Dominio -> DTO (para guardar devolver al frontend)
        CreateMap<TipoLogro, TipoLogroDTO>();
        
        // 4. DTO -> Mapa de Dominio (para mandar al backend)
        CreateMap<TipoLogroDTO, TipoLogro>();
    }
}
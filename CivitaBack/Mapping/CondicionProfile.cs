using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class CondicionProfile : Profile
{
    public CondicionProfile()
    {
        // 1. Mapa de EF -> Dominio (para leer de la base de datos)
        CreateMap<CondicionEF, Condicion>();

        // 2. Mapa de Dominio -> EF (para guardar en la base de datos)
        CreateMap<Condicion, CondicionEF>();
        
        // 3. Mapa de Dominio -> DTO (para guardar devolver al frontend)
        CreateMap<Condicion, CondicionDTO>();
        
        // 4. DTO -> Mapa de Dominio (para mandar al backend)
        CreateMap<CondicionDTO, Condicion>();
    }
}
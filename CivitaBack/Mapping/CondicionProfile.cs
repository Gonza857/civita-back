using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class CondicionProfile : Profile
{
    public CondicionProfile()
    {
        CreateMap<CondicionEF, Condicion>().ReverseMap();
        
        // 3. Mapa de Dominio -> DTO (para guardar devolver al frontend)
        CreateMap<Condicion, CondicionDTO>();
        
        // 4. DTO -> Mapa de Dominio (para mandar al backend)
        CreateMap<CondicionDTO, Condicion>();
        
        CreateMap<ActualizarCondicionDTO, Condicion>()
            
            // 1. Ignorar la clave primaria. Esto es crucial en cualquier UPDATE/PATCH.
            //    La clave nunca debe venir del cuerpo del mensaje.
            .ForMember(dest => dest.Id, opt => opt.Ignore())

            // 2. Ignorar el objeto de navegación Estructura (confiamos en EstructuraId)
            .ForMember(dest => dest.Estructura, opt => opt.Ignore()) 

            // 3. Ignorar la colección.
            //    Esto evita que AutoMapper intente adjuntar/insertar las recompensas
            //    de forma incorrecta. La lógica de sincronización debe manejar esto.
            .ForMember(dest => dest.Recompensas, opt => opt.Ignore())
            
            // 4. Ignorar los campos de Auditable que pudieran estar en el DTO (Creado, Editado)
            .ForMember(dest => dest.Creado, opt => opt.Ignore())
            .ForMember(dest => dest.Editado, opt => opt.Ignore());
    }
}
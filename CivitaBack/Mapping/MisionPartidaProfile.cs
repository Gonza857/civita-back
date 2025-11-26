using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class MisionPartidaProfile : Profile
{
    public MisionPartidaProfile()
    {
        CreateMap<MisionPartidaEF, MisionPartida>();
        CreateMap<MisionPartida, MisionPartidaEF>();
        
        CreateMap<MisionPartida, MisionPartidaDTO>()
            
            // --- 1a. Aplanamiento de Misión (Obtenemos Titulo y Descripción) ---
            // Le decimos a AutoMapper que acceda a la entidad anidada 'Mision' para obtener los datos.
            .ForMember(
                dest => dest.Titulo,
                opt => opt.MapFrom(src => src.Mision.Titulo)
            )
            .ForMember(
                dest => dest.Descripcion,
                opt => opt.MapFrom(src => src.Mision.Descripcion)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Mision.Id)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Mision.Id)
            )
            .ForMember(
                dest => dest.Tipo,
                opt => opt.MapFrom(src => src.Mision.Tipo.ToString())
            )
            // --- 1b. Nombre Desajustado (MisionPartida -> DTO) ---
            // La entidad tiene 'Reclamado', el DTO tiene 'Reclamada'.
            .ForMember(
                dest => dest.Reclamada,
                opt => opt.MapFrom(src => src.Reclamado)
            );
        
    }
}
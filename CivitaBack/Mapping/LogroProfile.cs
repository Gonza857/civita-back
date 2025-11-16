using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class LogroProfile : Profile
{
    public LogroProfile()
    {
        CreateMap<LogroDTO, Logro>()
            .ForMember(
                dest => dest.TipoLogroId, // El destino en tu entidad de dominio
                opt => opt.MapFrom(src => src.TipoId) // El origen en tu DTO
            );
        CreateMap<Logro, LogroDTO>()
            .ForMember(
                dest => dest.TipoId, // El destino en tu DTO
                opt => opt.MapFrom(src => src.TipoLogroId) // El origen en tu entidad de dominio
            )
            .ForMember(
                dest => dest.Tipo, // Asumiendo que tu DTO tiene un campo de string "Tipo"
                opt => opt.MapFrom(src => src.TipoLogro.Nombre) // Aplanando el nombre del tipo
            );
        CreateMap<Logro, LogroEF>().ReverseMap();
    }
}
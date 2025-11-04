using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping
{
    public class EstructuraMapaProfile : Profile
    {
        public EstructuraMapaProfile()
        {
            CreateMap<EstructuraMapaDTO, EstructuraMapa>()
            .ForMember(dest => dest.Partida, opt => opt.Ignore())
            .ForMember(dest => dest.Estructura, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<EstructuraMapa, EstructuraMapaEF>().ReverseMap();
            CreateMap<EstructuraMapa, EstructuraMapaDTO>();
            CreateMap<EstructuraMapaEF, EstructuraMapa>();
            CreateMap<EliminarEstructuraDTO, EstructuraMapa>();
        }
    }
}

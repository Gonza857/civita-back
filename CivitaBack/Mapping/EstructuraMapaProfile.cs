using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping
{
    public class EstructuraMapaProfile : Profile
    {
        public EstructuraMapaProfile() {
            CreateMap<EstructuraMapa, EstructuraMapaEF>();
            CreateMap<EstructuraMapaEF, EstructuraMapa>();
        }
    }
}

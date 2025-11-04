using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping
{
    public class TipPartidaProfile : Profile
    { 
        public TipPartidaProfile() {
            CreateMap<TipEnPartida, TipEnPartidaEF>();
            CreateMap<TipEnPartidaEF, TipEnPartida>();
        }
    }
}

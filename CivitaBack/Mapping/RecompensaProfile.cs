using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class RecompensaProfile : Profile
{
    public RecompensaProfile()
    {
        CreateMap<Recompensa, RecompensaEF>().ReverseMap();
        CreateMap<Recompensa, RecompensaDTO>().ReverseMap();
    }
}
using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class TipProfile : Profile
{
    public TipProfile()
    {
        CreateMap<TipEF, Tip>();
        CreateMap<Tip, TipEF>();
        CreateMap<Tip, TipDTO>();
        CreateMap<TipDTO, Tip>();
    }
}
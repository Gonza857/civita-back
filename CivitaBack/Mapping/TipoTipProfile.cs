using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class TipoTipProfile : Profile
{
    public TipoTipProfile()
    {
        CreateMap<TipoTipEF, TipoTip>();
        CreateMap<TipoTip, TipoTipEF>();
        CreateMap<TipoTip, TipoTipDTO>();
        CreateMap<TipoTipDTO, TipoTip>();

    }
}
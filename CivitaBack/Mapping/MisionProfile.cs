using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class MisionProfile : Profile
{
    public MisionProfile()
    {
        CreateMap<Mision, MisionEF>();
        CreateMap<MisionEF, Mision>();
        CreateMap<MisionDTO, Mision>();
        CreateMap<Mision, MisionDTO>();

    }
}
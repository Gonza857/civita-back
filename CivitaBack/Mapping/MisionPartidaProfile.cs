using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class MisionPartidaProfile : Profile
{
    public MisionPartidaProfile()
    {
        CreateMap<MisionPartidaEF, MisionPartida>();
        CreateMap<MisionPartida, MisionPartidaEF>();
    }
}
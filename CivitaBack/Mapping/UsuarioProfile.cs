using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<UsuarioEF, Usuario>();
        CreateMap<Usuario, UsuarioEF>();
    }
}
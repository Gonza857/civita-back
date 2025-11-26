using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<UsuarioEF, Usuario>()
            .ReverseMap();
        
        CreateMap<Usuario, UsuarioDTO>()
            .ForMember(
                dto => dto.Nombre, 
                opt => opt.MapFrom(src => src.NombreUsuario)
            );
        
        CreateMap<UsuarioDTO, Usuario>()
            .ForMember(
                dominio => dominio.NombreUsuario, 
                opt => opt.MapFrom(dto => dto.Nombre) 
            );
    }
}
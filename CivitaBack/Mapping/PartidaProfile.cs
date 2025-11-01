using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class PartidaProfile : Profile
{
    public PartidaProfile()
    {
        CreateMap<Partida, PartidaEF>();
        CreateMap<PartidaEF, Partida>();
        CreateMap<Partida, PartidaDTO>()
            
            // --- Aplanamiento de Recursos ---
            // Le decimos a AutoMapper que busque dentro del objeto 'Recursos'.
            // AutoMapper es lo suficientemente inteligente para manejar si 'Recursos' es null
            // (en ese caso, 'Energia' en el DTO será '0' (el valor por defecto de un int)).
            
            .ForMember(
                dto => dto.Energia, 
                opt => opt.MapFrom(dominio => dominio.Recursos.Energia)
            )
            .ForMember(
                dto => dto.Felicidad, // Asumo que 'Felicidad' también está en Recursos
                opt => opt.MapFrom(dominio => dominio.Recursos.Felicidad)
            )
            .ForMember(
                dto => dto.Contaminacion,
                opt => opt.MapFrom(dominio => dominio.Recursos.Contaminacion)
            )
            .ForMember(
                dto => dto.EcoCoins, 
                opt => opt.MapFrom(dominio => dominio.Recursos.EcoCoins)
            )
            
            // --- Aplanamiento de Usuario ---
            // Asumo que la propiedad 'Usuario' (string) en el DTO
            // debe ser el nombre del objeto 'Usuario' en el dominio.
            .ForMember(
                dto => dto.Usuario, 
                opt => opt.MapFrom(dominio => dominio.Usuario.NombreUsuario) // O .Username, .Email, etc.
            )
            
            // --- Ignorar Propiedades Problemáticas ---
            // Ignoramos la referencia circular 'Partida -> Partida' en el DTO
            .ForMember(
                dto => dto.Partida, 
                opt => opt.Ignore()
            );
    }
}
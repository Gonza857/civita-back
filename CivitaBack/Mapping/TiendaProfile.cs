using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping
{
    public class TiendaProfile : Profile
    {
        public TiendaProfile()
        {
            CreateMap<Tienda, TiendaEF>();
            CreateMap<TiendaEF, Tienda>();
            // Mapeo: Entidad de Dominio (Estructura) -> DTO de Salida (TiendaItemDTO)
            CreateMap<Estructura, TiendaDTO>()

                // 1. Mapeo de Identificadores (Aplanamiento del ID)
                // La entidad Estructura.Id se mapea a TiendaItemDTO.EstructuraId
                .ForMember(
                    dest => dest.EstructuraId,
                    opt => opt.MapFrom(src => src.Id)
                )

                // 2. Mapeos Directos (AutoMapper los haría automáticamente, pero es bueno ser explícito aquí)
                .ForMember(
                    dest => dest.Nombre,
                    opt => opt.MapFrom(src => src.Nombre)
                )
                .ForMember(
                    dest => dest.RutaImagen,
                    opt => opt.MapFrom(src => src.RutaImagen)
                )

                // 3. Mapeo de Costos (Aplanamiento de Costo)
                .ForMember(
                    dest => dest.CostoDinero,
                    opt => opt.MapFrom(src => src.CostoDinero)
                )

                // 4. Mapeo de Propiedades Anidadas (si quieres el nombre del Tipo)

                .ForMember(
                dest => dest.TipoEstructuraId,
                opt => opt.MapFrom(src => src.TipoEstructura.Id)
                )
                .ForMember(
                dest => dest.TipoNombre,
                opt => opt.MapFrom(src => src.TipoEstructura.Nombre)
                )

                // 5. Mapeo de efectos por ciclo
                .ForMember(
                dest => dest.FelicidadCiclo,
                opt => opt.MapFrom(src => src.FelicidadCiclo)
                )
                .ForMember(
                dest => dest.ContaminacionCiclo,
                opt => opt.MapFrom(src => src.ContaminacionCiclo)
                )

                .ForMember(
                dest => dest.DineroCiclo,
                opt => opt.MapFrom(src => src.TipoEstructura.DineroPorCiclo)
                )
                .ForMember(
                dest => dest.EnergiaCiclo,
                opt => opt.MapFrom(src => src.TipoEstructura.EnergiaPorCiclo)
                );

        }
    }
}

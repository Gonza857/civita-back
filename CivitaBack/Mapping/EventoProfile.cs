using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            CreateMap<Evento, EventoEF>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EventoMaestro, opt => opt.Ignore())
            .ForMember(dest => dest.EventoMaestroId, opt => opt.MapFrom(src => src.EventoMaestro != null ? src.EventoMaestro.Id : src.EventoMaestroId));

            CreateMap<EventoEF, Evento>();
            CreateMap<Evento, EventoMaestro>();

            CreateMap<EventoMaestro, Evento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EventoMaestro, opt => opt.Ignore());

            CreateMap<Evento, EventoDisparadoDTO>()
            .ForMember(
                dest => dest.Titulo,
                opt => opt.MapFrom(src => src.EventoMaestro != null ? src.EventoMaestro.Nombre : "Evento")
            );

            CreateMap<Evento, EventoResueltoDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PartidaId, opt => opt.MapFrom(src => src.PartidaId))

            .ForMember(
            dest => dest.Texto,
            opt => opt.MapFrom((src, dest, _, context) => {
            bool aceptado = (bool)context.Items["Aceptado"];
            return aceptado ? src.TextoAceptar : src.TextoRechazar;
            })
            );

        }

    }
}

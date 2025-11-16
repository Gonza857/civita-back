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
            /*CreateMap<Evento, EventoEF>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EventoMaestro, opt => opt.Ignore())
            .ForMember(dest => dest.EventoMaestroId, opt => opt.MapFrom(src => src.EventoMaestro != null ? src.EventoMaestro.Id : src.EventoMaestroId));*/

            CreateMap<EventoMaestro, Evento>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Contenido, opt => opt.MapFrom(src => src.ContenidoPrincipal))

            .ForMember(dest => dest.EcoCoinsAplicada, opt => opt.Ignore())
            .ForMember(dest => dest.FelicidadAplicada, opt => opt.Ignore())
            .ForMember(dest => dest.ContaminacionAplicada, opt => opt.Ignore())
            .ForMember(dest => dest.EnergiaAplicada, opt => opt.Ignore())
            .ForMember(dest => dest.ExperienciaAplicada, opt => opt.Ignore())

            .ForMember(dest => dest.EventoMaestroId, opt => opt.Ignore())
            .ForMember(dest => dest.EventoMaestro, opt => opt.Ignore())
            .ForMember(dest => dest.PartidaId, opt => opt.Ignore())
            .ForMember(dest => dest.SeDisparo, opt => opt.Ignore())
            .ForMember(dest => dest.Resuelto, opt => opt.Ignore());

            CreateMap<Evento, EventoEF>().ReverseMap();
            CreateMap<EventoMaestro, EventoMaestroEF>().ReverseMap();
            CreateMap<EfectoEvento, EfectoEventoEF>().ReverseMap();

            CreateMap<Evento, EventoDisparadoDTO>()
            .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.EventoMaestro.Titulo))
            .ForMember(dest => dest.PreguntaTexto, opt => opt.MapFrom(src => src.EventoMaestro.ContenidoPrincipal))
            .ForMember(dest => dest.OpcionA_Texto, opt => opt.MapFrom(src => src.EventoMaestro.OpcionA_Texto))
            .ForMember(dest => dest.OpcionB_Texto, opt => opt.MapFrom(src => src.EventoMaestro.OpcionB_Texto))
            .ForMember(dest => dest.EventoMaestroId, opt => opt.MapFrom(src => src.EventoMaestroId));


            CreateMap<Evento, EventoResueltoDTO>()
            .ForMember(dest => dest.PartidaId, opt => opt.MapFrom(src => src.PartidaId))
            .ForMember(
                dest => dest.TextoRespuesta,
                opt => opt.MapFrom((src, dest, _, context) =>
                {
                    bool esAcierto = src.RespuestaJugador == src.EventoMaestro.RespuestaCorrecta;
                    return esAcierto
                        ? $"¡Respuesta correcta! {src.EventoMaestro.Titulo} ha sido resuelto."
                        : $"Respuesta incorrecta. {src.EventoMaestro.Titulo} ha sido resuelto.";
                })
            );


        }

    }
}

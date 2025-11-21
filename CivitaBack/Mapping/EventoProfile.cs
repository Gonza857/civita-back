using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Enum;

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

            CreateMap<Evento, EventoEF>()
            .ForMember(dest => dest.RespuestaJugador, opt => opt.MapFrom(src => src.RespuestaJugador))
            .ForMember(dest => dest.EventoMaestro, opt => opt.Ignore())
            .ForMember(dest => dest.Partida, opt => opt.Ignore())
            .ReverseMap();

            CreateMap<EventoMaestro, EventoMaestroEF>().ReverseMap();
            CreateMap<EfectoEvento, EfectoEventoEF>().ReverseMap();

            CreateMap<EventoMaestro, EventoDisparadoDTO>()
            .ForMember(dest => dest.TipoEvento, opt => opt.MapFrom(src => src.TipoEvento.ToString()))
            .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
            .ForMember(dest => dest.PreguntaTexto, opt => opt.MapFrom(src => src.ContenidoPrincipal))
            .ForMember(dest => dest.OpcionA_Texto, opt => opt.MapFrom(src => src.OpcionA_Texto))
            .ForMember(dest => dest.OpcionB_Texto, opt => opt.MapFrom(src => src.OpcionB_Texto))
            .ForMember(dest => dest.EfectoAciertoResumen,
                       opt => opt.MapFrom(src =>
                            FormatearEfectos(
                                src.Efectos != null
                                    ? src.Efectos.FirstOrDefault(e => e.TipoResultado == TipoResultado.ACIERTO)
                                    : null
                            )
                       ));

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

        private string FormatearEfectos(EfectoEvento? ef)
        {
            if (ef == null) return string.Empty;

            return $"+{ef.EcoCoins} EcoCoins, +{ef.Felicidad} Felicidad, {ef.Contaminacion} Contaminación, +{ef.Energia} Energía";
        }
    }

}

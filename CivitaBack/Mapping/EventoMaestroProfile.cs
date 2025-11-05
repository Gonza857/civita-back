using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping
{
    public class EventoMaestroProfile : Profile
    {
        public EventoMaestroProfile()
        {
            CreateMap<EventoMaestroEF, EventoMaestro>();
            CreateMap<EventoMaestro, EventoMaestroEF>();
        }
    }
}

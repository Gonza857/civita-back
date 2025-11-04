using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping
{
    public class EventoProfile : Profile
    {
        public EventoProfile()
        {
            CreateMap<Evento, EventoEF>();
            CreateMap<EventoEF, Evento>();
        }

    }
}

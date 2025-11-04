using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping
{
    public class LogroPartidaProfile : Profile
    {
        public LogroPartidaProfile()
        {
            CreateMap<LogroPartida, LogroPartidaEF>();
            CreateMap<LogroPartidaEF, LogroPartida>();
        }
    }
}

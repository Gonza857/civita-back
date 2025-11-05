using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class RecursoProfile : Profile
{
    public RecursoProfile()
    {
        CreateMap<Recurso, RecursoEF>();
        CreateMap<RecursoEF, Recurso>();
        CreateMap<Recurso, RecursoDTO>();
        CreateMap<RecursoDTO, Recurso>();
    }
}
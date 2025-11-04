using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class RecursoProfile : Profile
{
    public RecursoProfile()
    {
        CreateMap<Recurso, RecursoEF>();
        CreateMap<RecursoEF, Recurso>();
    }
}
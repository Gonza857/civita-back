using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class TipProfile : Profile
{
    public TipProfile()
    {
        // 1. Mapa de EF -> Dominio (para leer de la base de datos)
        CreateMap<TipEF, Tip>();

        // 2. Mapa de Dominio -> EF (para guardar en la base de datos)
        CreateMap<Tip, TipEF>();
        
        // 2. Mapa de Dominio -> DTO (para mandar al frontend)
        CreateMap<Tip, TipDTO>();
    }
}
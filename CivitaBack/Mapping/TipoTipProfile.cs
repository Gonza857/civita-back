using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Api.Mapping;

public class TipoTipProfile : Profile
{
    public TipoTipProfile()
    {
        // 1. Mapa de EF -> Dominio (para leer de la base de datos)
        CreateMap<TipoTipEF, TipoTip>();

        // 2. Mapa de Dominio -> EF (para guardar en la base de datos)
        CreateMap<TipoTip, TipoTipEF>();
        
        // 2. Mapa de Dominio -> DTO (para mandar al frontend)
        CreateMap<TipoTip, TipoTipDTO>();
    }
}
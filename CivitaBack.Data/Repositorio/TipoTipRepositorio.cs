using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio;

public class TipoTipRepositorio 
    : GenericoRepositorio<TipoTip, TipoTipEF>, ITipoTipRepositorio 
{
    public TipoTipRepositorio (AppDbContext context, IMapper mapper) : base(context, mapper) { }
    

    
}
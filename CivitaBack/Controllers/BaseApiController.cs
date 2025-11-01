using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CivitaBack.Api.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    // 1. Hacemos el mapper "protected" para que las clases hijas lo vean
    protected readonly IMapper _mapper;

    // 2. El constructor de la clase base recibe el mapper
    protected BaseApiController(IMapper mapper)
    {
        _mapper = mapper;
    }

    // 3. Tu método helper (usando _mapper)
    protected TDestino Mapear<TDestino>(object? source)
    {
        return _mapper.Map<TDestino>(source);
    }

    // 4. Tu método helper para listas (usando _mapper)
    protected List<TDestino> MapearLista<TDestino>(object sourceList)
    {
        return _mapper.Map<List<TDestino>>(sourceList);
    }
}
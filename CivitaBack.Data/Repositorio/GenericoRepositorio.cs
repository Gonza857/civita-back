using System.Linq.Expressions;
using CivitaBack.Data.EF;
using AutoMapper;
using CivitaBack.Data.BO;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Utils;

namespace CivitaBack.Data.Repositorio;

public abstract class GenericoRepositorio<TDominio, TEf> : IRepositorioBase<TDominio>
    where TDominio : class
    where TEf : class
{
    protected readonly AppDbContext _context;
    protected readonly IMapper _mapper;
    protected readonly DbSet<TEf> _dbSet; // Un DbSet genérico

    public GenericoRepositorio(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _dbSet = _context.Set<TEf>(); 
    }
    
    public async Task Agregar(TDominio entidad)
    {
        var entidadEF = _mapper.Map<TEf>(entidad);
        if (entidadEF is AuditableEF auditable)
        {
            auditable.Creado = DateTime.UtcNow;
            auditable.Editado = DateTime.UtcNow; // También se setea al crear
        }
        await _dbSet.AddAsync(entidadEF);
    }

    public Task AgregarVarios(ICollection<TDominio> entidades)
    {
        var entidadesEF = _mapper.Map<IEnumerable<TEf>>(entidades);
        _dbSet.AddRange(entidadesEF);
        return Task.CompletedTask;
    }

    public Task Actualizar(TDominio entidad)
    {
        var entidadEF = _mapper.Map<TEf>(entidad);
        if (entidadEF is AuditableEF auditable) auditable.Editado = DateTime.UtcNow;
        _dbSet.Update(entidadEF); 
        return Task.CompletedTask;
    }
    
    public async Task Eliminar(int id)
    {
        // Usamos FindAsync que es perfecto para buscar por PK
        var entidadEF = await _dbSet.FindAsync(id);
        if (entidadEF != null)
        {
            _dbSet.Remove(entidadEF);
        }
    }
    
    /// <summary>
    /// Prepara la eliminación de una colección de entidades.
    /// </summary>
    /// <param name="entidades">La colección de entidades de Dominio a eliminar.</param>
    public Task EliminarVarios(ICollection<TDominio> entidades)
    {
        var entidadesEf = _mapper.Map<IEnumerable<TEf>>(entidades);
        _dbSet.RemoveRange(entidadesEf);
        return Task.CompletedTask;
    }
    
    public Task ActualizarVarios(ICollection<TDominio> entidades)
    {
        // 1. Mapea la lista de Dominio -> EF
        var entidadesEF = _mapper.Map<IEnumerable<TEf>>(entidades);
    
        // 2. Le dice a EF que TODAS estas entidades están "Modificadas"
        _dbSet.UpdateRange(entidadesEF);
    
        return Task.CompletedTask;
    }
    
    // --- MÉTODOS "VIRTUALES" ---
    // (Estos pueden ser sobreescritos por las clases hijas si necesitan
    // lógica especial, como los `Include`s)

    public virtual async Task<TDominio?> ObtenerPorId(Expression<Func<TEf, bool>> predicado)
    {
        var entidadEF = await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicado);
        return _mapper.Map<TDominio>(entidadEF);
    }
    
    public virtual async Task<List<TDominio>> ObtenerVariosPor(Expression<Func<TEf, bool>> predicado)
    {
        var listaEF = await _dbSet.AsNoTracking().Where(predicado).ToListAsync();
        return _mapper.Map<List<TDominio>>(listaEF);
    }
    
    public virtual async Task<List<TDominio>> ObtenerTodos()
    {
        // Implementación base: solo trae todos sin Includes
        var listaEF = await _dbSet.AsNoTracking().ToListAsync();
        return _mapper.Map<List<TDominio>>(listaEF);
    }

    /// <summary>
    /// Mapea un objeto de origen a un nuevo objeto de destino.
    /// </summary>
    /// <typeparam name="TDestino">El tipo de destino (ej: Domain.Entities.Logro)</typeparam>
    /// <param name="source">El objeto de origen (ej: Data.BO.Logro)</param>
    /// <returns>Un nuevo objeto de tipo TDestino.</returns>
    protected TDestino Mapear<TDestino>(object? source)
    {
        return _mapper.Map<TDestino>(source);
    }

    /// <summary>
    /// Mapea una lista de objetos de origen a una nueva lista de destino.
    /// </summary>
    protected List<TDestino> MapearLista<TDestino>(object sourceList)
    {
        return _mapper.Map<List<TDestino>>(sourceList);
    }
    
}
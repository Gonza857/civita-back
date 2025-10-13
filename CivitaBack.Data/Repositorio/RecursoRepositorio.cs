using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;

namespace CivitaBack.Data.Repositorio;
public interface IRecursoRepositorio : IRepositorioBase<Recurso>
{
    void GuardarVarios(List<Recurso> recursos);
    List<Recurso> ObtenerRecursosPartida(int idPartida);
}
public class RecursoRepositorio : IRecursoRepositorio
{
    private readonly AppDbContext _context;

    public RecursoRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public void Actualizar(Recurso entidad)
    {
        _context.Recurso.Update(entidad);
        _context.SaveChanges();
    }
    public void Eliminar(int id)
    {
        throw new NotImplementedException();
    }

    public void Guardar(Recurso entidad)
    {
        throw new NotImplementedException();
    }

    public void GuardarVarios(List<Recurso> recursos)
    {
        _context.Recurso.AddRange(recursos);
        _context.SaveChanges();
    }

    public Recurso ObtenerPorId(int id)
    {
        throw new NotImplementedException();
    }

    public List<Recurso> ObtenerRecursosPartida(int idPartida)
    {
        return _context.Recurso
           .Where(r => r.PartidaId == idPartida)
           .ToList();
    }

    public List<Recurso> ObtenerTodos()
    {
        throw new NotImplementedException();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface ILogroRepositorio : IRepositorioBase<Logro>
{
    void Actualizar();
}
public class LogroRepositorio : ILogroRepositorio
{
    private readonly AppDbContext _context;

    public LogroRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public void Actualizar(Logro entidad)
    {
        throw new NotImplementedException();
    }

    public void Actualizar()
    {
        _context.SaveChanges();
    }

    public void Eliminar(int id)
    {
        var logro = _context.Logro.FirstOrDefault(tl => tl.Id == id);
        if (logro != null)
        {
            _context.Logro.Remove(logro);
            _context.SaveChanges();
        }
    }

    public void Guardar(Logro entidad)
    {
        _context.Logro.Add(entidad);
        _context.SaveChanges();
    }

    public Logro ObtenerPorId(int id)
    {
        return _context.Logro
            .Include(tl => tl.TipoLogro)
            .FirstOrDefault(tl => tl.Id == id);
    }

    public List<Logro> ObtenerTodos()
    {
        return _context.Logro
            .Include (tl => tl.TipoLogro)
            .ToList();
    }
}

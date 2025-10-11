using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;

namespace CivitaBack.Data.Repositorio;

public interface ITipoLogroRepositorio
{
    void Guardar(TipoLogro TipoLogro);
    List<TipoLogro> ObtenerTodos();
    TipoLogro ObtenerPorId(int Id);
    void Eliminar(int Id);

    void Actualizar();
}

public class TipoLogroRepositorio : ITipoLogroRepositorio
{
    private readonly AppDbContext _context;

    public TipoLogroRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public void Actualizar()
    {
        _context.SaveChanges();
    }

    public void Eliminar(int Id)
    {
        var tipoLogro = _context.TipoLogro.FirstOrDefault(tl => tl.Id == Id);
        if (tipoLogro != null)
        {
            _context.TipoLogro.Remove(tipoLogro);
            _context.SaveChanges();
        }
    }

    public void Guardar(TipoLogro TipoLogro)
    {
        _context.TipoLogro.Add(TipoLogro);
        _context.SaveChanges();
    }

    public TipoLogro ObtenerPorId(int Id)
    {
        return _context.TipoLogro.FirstOrDefault(tl => tl.Id == Id);
    }

    public List<TipoLogro> ObtenerTodos()
    {
        return _context.TipoLogro.ToList();
    }
}

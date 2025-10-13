using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;

namespace CivitaBack.Data.Repositorio;

public interface IEstructuraMapaRepositorio
{
    void Guardar(EstructuraMapa em);
}

public class EstructuraMapaRepositorio : IEstructuraMapaRepositorio
{
    private readonly AppDbContext _context;

    public EstructuraMapaRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public void Guardar(EstructuraMapa em)
    {
        _context.EstructuraMapa.Add(em);
        _context.SaveChanges();
    }
}

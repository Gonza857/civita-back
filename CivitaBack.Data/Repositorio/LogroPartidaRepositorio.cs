using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface ILogroPartidaRepositorio
{
    List<Logro> ObtenerLogrosIncompletos(int partidaId);
    List<Logro> ObtenerLogrosCompletos(int partidaId);
}

public class LogroPartidaRepositorio : ILogroPartidaRepositorio
{
    private readonly AppDbContext _context;

    public LogroPartidaRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public List<Logro> ObtenerLogrosCompletos(int partidaId)
    {
        //SELECT*
        //FROM Logros l
        //WHERE EXISTS(
        //    SELECT 1
        //    FROM LogroPartida lp
        //    WHERE lp.LogroId = l.Id
        //      AND lp.PartidaId = @partidaId
        //);
        return _context.Logro
            .Include(l => l.TipoLogro)
            .Where(l => l.LogroPartidas.Any(lp => lp.PartidaId == partidaId))
            .ToList();
    }

    public List<Logro> ObtenerLogrosIncompletos(int partidaId)
    {
        //SELECT*
        //FROM Logros l
        //WHERE NOT EXISTS(
        //    SELECT 1
        //    FROM LogroPartida lp
        //    WHERE lp.LogroId = l.Id
        //      AND lp.PartidaId = @partidaId
        //);
        return _context.Logro
            .Include(l => l.TipoLogro)
            .Where(l => !l.LogroPartidas.Any(lp => lp.PartidaId == partidaId))
            .ToList();

    }
}

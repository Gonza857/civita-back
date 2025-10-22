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
    void AgregarUnica(EstructuraMapa em);
    void RemoverEliminadas(List<EstructuraMapa> emList);
    void AgregarNuevas(List<EstructuraMapa> emList);
    Task GuardarCambios();
    Task EliminarPorPartidaIdAsync(int partidaId);
    Task AgregarVariasAsync(List<EstructuraMapa> estructuras);

}

public class EstructuraMapaRepositorio : GenericoRepositorio, IEstructuraMapaRepositorio
{
    public EstructuraMapaRepositorio(AppDbContext context) : base(context) { }
    
    

    public void AgregarUnica(EstructuraMapa em)
    {
        em.Editado = DateTime.UtcNow;
        _context.EstructuraMapa.Update(em);
    }

    public void RemoverEliminadas(List<EstructuraMapa> emList)
    {
        _context.EstructuraMapa.RemoveRange(emList);
    }

    public void AgregarNuevas(List<EstructuraMapa> emList)
    {
        _context.EstructuraMapa.AddRange(emList);
    }
    
    public async Task GuardarCambios()
    {
        await base.GuardarCambiosAsync();
    }

    public async Task EliminarPorPartidaIdAsync(int partidaId)
    {
        var existentes = _context.EstructuraMapa.Where(e => e.PartidaId == partidaId);
        _context.EstructuraMapa.RemoveRange(existentes);
        await _context.SaveChangesAsync();
    }

    public async Task AgregarVariasAsync(List<EstructuraMapa> estructuras)
    {
        await _context.EstructuraMapa.AddRangeAsync(estructuras);
        await _context.SaveChangesAsync();
    }


}

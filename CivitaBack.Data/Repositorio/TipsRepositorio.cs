using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;

namespace CivitaBack.Data.Repositorio
{
    public interface ITipsRepositorio
    {
        List<Tip> ObtenerMsjPorIdTipo(int idTipo);
        IEnumerable<TipoTip> GetTiposTips();
    }
    public class TipsRepositorio : ITipsRepositorio
    {

        private readonly AppDbContext _context;

        
        public TipsRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public List<Tip> ObtenerMsjPorIdTipo(int idTipo)
        {
            return  _context.Tip.Where(x => x.TipoId == idTipo).ToList();
        }

        public IEnumerable<TipoTip> GetTiposTips()
        {
            return _context.TipoTip.ToList();
        }
    }
}

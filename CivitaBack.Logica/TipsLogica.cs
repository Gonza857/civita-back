using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;

namespace CivitaBack.Logica
{

    public interface ITipsLogica
    {
        List<TipsDTO> ObtenerMsjPorIdTipo(int id);
        
        List<TipoTipDTO> GetTiposTips();
    }
    public class TipsLogica : ITipsLogica
    {
        private readonly ITipsRepositorio tipsRepositorio;
        public TipsLogica(ITipsRepositorio itr)
        {
            tipsRepositorio = itr;
        }

        public List<TipsDTO> ObtenerMsjPorIdTipo(int id)
        {
                List<TipsDTO> listaDto = new List<TipsDTO>();
                var entity = tipsRepositorio.ObtenerMsjPorIdTipo(id);
                listaDto = entity.Select(t=> TipsToDTO(t)).ToList();
                return listaDto;
        }

        public List<TipoTipDTO> GetTiposTips()
        {
            List<TipoTipDTO> listaDto = new List<TipoTipDTO>();
            var entity = tipsRepositorio.GetTiposTips(); 
            listaDto= entity.Select(t=>TipoTipsToDTO(t)).ToList();
            return listaDto;
        }


        private TipsDTO TipsToDTO(Tip E)
        {
            TipsDTO tipsDTO = new TipsDTO();
            tipsDTO.Id = E.Id;
            tipsDTO.Mensaje = E.Mensaje;
            tipsDTO.TipoId = E.TipoId;
            tipsDTO.ElementoAdicional = E.ElementoAdicional;
            tipsDTO.Expresion =  E.Expresion;
            tipsDTO.EfectoFiltro = E.EfectoFiltro; 
            return tipsDTO;
        }

        private TipoTipDTO TipoTipsToDTO(TipoTip E)
        {
            TipoTipDTO tipoDto = new TipoTipDTO();
            tipoDto.Id = E.Id;
            tipoDto.Descripcion  = E.Descripcion;
            return tipoDto;
        }

    }
}

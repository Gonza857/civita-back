using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivitaBack.Logica
{

    public interface IUsuarioLogica
    {
        Task<Usuario> ObtenerPorId(int id);
        Task<Usuario> ObtenerUsuarioPorNombre(string nombre);

    }
    public class UsuarioLogica : IUsuarioLogica
    {
        private readonly IUsuarioRepositorio _repositorioUsuario;

        public UsuarioLogica(IUsuarioRepositorio repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        public Task<Usuario> ObtenerPorId(int id)
        {
            Task<Usuario> usuario = this._repositorioUsuario.ObtenerPorId(id);
            if (usuario == null) throw new Exception("No se encontró el usuario");
            return usuario;
        }

        public async Task<Usuario> ObtenerUsuarioPorNombre(string nombre)
        {
            Usuario usuario = await _repositorioUsuario.ObtenerUsuarioPorNombre(nombre);
            if (usuario == null) throw new Exception("No se encontró el usuario");
            return usuario;
        }

    }
}

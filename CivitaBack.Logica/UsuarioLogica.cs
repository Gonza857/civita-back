using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Logica
{
    public class UsuarioLogica : IUsuarioLogica
    {
        private readonly IUsuarioRepositorio _repositorioUsuario;

        public UsuarioLogica(IUsuarioRepositorio repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        public async Task<Usuario> ObtenerPorId(int id)
        {
            Usuario? usuario = await this._repositorioUsuario.ObtenerPorId(id);
            if (usuario == null) throw new DominioException("No se encontró el usuario");
            return usuario;
        }

        public async Task<Usuario> ObtenerPorCorreo(string correo)
        {
            var usuario = await this._repositorioUsuario.ObtenerUsuarioPorMail(correo);
            if (usuario == null)
                throw new UsuarioExcepcion("No se encontró el usuario");
            return usuario;
        }

        public async Task<Usuario?> ObtenerUsuarioPorNombre(string nombre)
        {
            Usuario? usuario = await _repositorioUsuario.ObtenerUsuarioPorNombre(nombre);
            // if (usuario == null) throw new UsuarioExcepcion("No se encontró el usuario");
            return usuario;
        }

    }
}

using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Logica
{
    public class UsuarioLogica : IUsuarioLogica
    {
        private readonly IUsuarioRepositorio _repositorioUsuario;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UsuarioLogica"/>.
        /// </summary>
        /// <param name="repositorioUsuario">El repositorio para acceder a los datos de usuario.</param>
        public UsuarioLogica(IUsuarioRepositorio repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        /// <inheritdoc />
        public async Task<Usuario> ObtenerPorId(int id)
        {
            Usuario? usuario = await this._repositorioUsuario.ObtenerPorId(id);
            if (usuario == null) throw new DominioException("No se encontró el usuario");
            return usuario;
        }

        /// <inheritdoc />
        public async Task<Usuario> ObtenerPorCorreo(string correo)
        {
            var usuario = await this._repositorioUsuario.ObtenerUsuarioPorMail(correo);
            if (usuario == null)
                throw new UsuarioExcepcion("No se encontró el usuario");
            return usuario;
        }

        /// <inheritdoc />
        public async Task<Usuario?> ObtenerUsuarioPorNombre(string nombre)
        {
            Usuario? usuario = await _repositorioUsuario.ObtenerUsuarioPorNombre(nombre);
            // El método está diseñado para devolver null si no existe, según la firma.
            return usuario;
        }

    }
}

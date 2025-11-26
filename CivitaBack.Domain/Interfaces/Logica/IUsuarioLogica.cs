using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica
{
    public interface IUsuarioLogica
    {
        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario.</param>
        /// <returns>La entidad Usuario.</returns>
        /// <exception cref="DominioException">Se lanza si no se encuentra el usuario con el ID proporcionado.</exception>
        Task<Usuario> ObtenerPorId(int id);

        /// <summary>
        /// Obtiene un usuario por su dirección de correo electrónico.
        /// </summary>
        /// <param name="correo">La dirección de correo electrónico.</param>
        /// <returns>La entidad Usuario.</returns>
        /// <exception cref="UsuarioExcepcion">Se lanza si no se encuentra el usuario con el correo proporcionado.</exception>
        Task<Usuario> ObtenerPorCorreo(string correo);

        /// <summary>
        /// Obtiene un usuario por su nombre de usuario.
        /// </summary>
        /// <param name="nombre">El nombre de usuario.</param>
        /// <returns>La entidad Usuario, o null si no se encuentra.</returns>
        Task<Usuario?> ObtenerUsuarioPorNombre(string nombre);

    }
}

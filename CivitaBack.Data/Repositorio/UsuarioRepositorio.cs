using CivitaBack.Domain.Entities;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;
using CivitaBack.Domain.Interfaces.Repositorios;

namespace CivitaBack.Data.Repositorio
{

    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly AppDbContext _context;
       

        public UsuarioRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObtenerUsuarioPorMail(string mail)
        {
            return await _context.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Mail == mail);
        }

        public async Task<Usuario?> ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            return await _context.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public Task<Usuario> ObtenerPorId(int id)
        {
            return _context.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }
        
        public async Task<List<Usuario>> ObtenerTodosLosUsuarios()
        {
            return await _context.Usuario.AsNoTracking().ToListAsync();
        }

    }
}

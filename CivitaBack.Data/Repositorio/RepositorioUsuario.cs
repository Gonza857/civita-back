using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Data.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio
{
    public interface IRepositorioUsuario
    {
        Task<Usuario> obtenerUsuarioPorMail(string mail);
        Task<Usuario> obtenerUsuarioPorNombre(string nombreUsuario);
        Task<Usuario> CrearUsuario(Usuario usuario);
    }

    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly AppDbContext _context;

        public RepositorioUsuario(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> obtenerUsuarioPorMail(string mail)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Mail == mail);
        }

        public async Task<Usuario> obtenerUsuarioPorNombre(string nombreUsuario)
        {
            return await _context.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}

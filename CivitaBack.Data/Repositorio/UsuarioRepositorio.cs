using AutoMapper;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public class UsuarioRepositorio
    : GenericoRepositorio<Usuario, UsuarioEF>, IUsuarioRepositorio
{
    public UsuarioRepositorio(AppDbContext context, IMapper mapper) : base(context, mapper) { }
    
    public async Task<Usuario?> ObtenerUsuarioPorMail(string mail)
    {
        var usuarioEf = await _context.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Mail == mail);
        return base.Mapear<Usuario>(usuarioEf);
    }

    public async Task<Usuario?> ObtenerUsuarioPorNombre(string nombreUsuario)
    {
        var usuarioEf = await _context.Usuario
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        return base.Mapear<Usuario>(usuarioEf);
    }

    public async Task<Usuario> CrearUsuario(Usuario usuario)
    {
        await base.Agregar(usuario);
        return usuario;
    }

    public async Task<Usuario?> ObtenerPorId(int id)
    {
        var usuario = await base.ObtenerPorId(e => e.Id == id);
        return base.Mapear<Usuario>(usuario);
    }
}
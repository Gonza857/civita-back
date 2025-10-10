using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;

namespace CivitaBack.Data.Repositorio;

public interface IRepositorioPartida
{
    Partida ObtenerPorUsuarioId(int IdUsuario);

    Partida CrearPartida(Usuario usuario);
    List<Partida> ObtenerPartidas();
}
public class RepositorioPartida : IRepositorioPartida
{

    private readonly AppDbContext _context;

    public RepositorioPartida(AppDbContext context)
    {
        _context = context;
    }


    public Partida CrearPartida(Usuario usuario)
    {
        _context.Add(usuario);
        _context.SaveChanges();

        Partida partida = new Partida
        {
            UsuarioId = usuario.Id
        };
        _context.Add(partida);
        _context.SaveChanges();
        return partida;
    }

    public List<Partida> ObtenerPartidas()
    {
        return _context.Partida.ToList();
    }

    public Partida ObtenerPorUsuarioId(int IdUsuario)
    {
        return _context.Partida.FirstOrDefault((p) => p.UsuarioId == IdUsuario);
    }
}

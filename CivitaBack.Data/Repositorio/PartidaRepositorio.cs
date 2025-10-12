    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio;

public interface IRepositorioPartida
{
    Partida ObtenerPorUsuarioId(int IdUsuario);

    Partida CrearPartida(int idUsuario);
    List<Partida> ObtenerPartidas();

    void Guardar(Partida partida);
    void Actualizar();
}
public class PartidaRepositorio : IRepositorioPartida
{

    private readonly AppDbContext _context;

    public PartidaRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public void Actualizar()
    {
        _context.SaveChanges();
    }

    public Partida CrearPartida(int idUsuario)
    {
        Partida partida = new Partida
        {
            UsuarioId = idUsuario
        };
        _context.Add(partida);
        _context.SaveChanges();
        return partida;
    }

    public void Guardar(Partida partida)
    {
        _context.Add(partida);
        _context.SaveChanges();
    }

    public List<Partida> ObtenerPartidas()
    {
        return _context.Partida
            .Include(p => p.Usuario)
            .Include(p => p.Recursos)
            .ToList();
    }

    public Partida ObtenerPorUsuarioId(int IdUsuario)
    {
        return _context.Partida
            .Include(p => p.Recursos)
            .Include(p => p.Usuario)
            .FirstOrDefault((p) => p.UsuarioId == IdUsuario);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.EF;
using CivitaBack.Data.Repositorio;

namespace CivitaBack.Logica;

public interface IPartidaLogica
{
    PartidaDTO ObtenerPorUsuarioId(int IdUsuario);
    Partida CrearPartida(int idUsuario);

    List<PartidaDTO> ObtenerPartidas();
}

public class PartidaLogica : IPartidaLogica
{

    private readonly IRepositorioPartida repositorioPartida;
    
    public PartidaLogica(IRepositorioPartida repositoriopartida)
    {
        repositorioPartida = repositoriopartida;
    }

    public Partida CrearPartida(int idUsuario)
    {
        Usuario usuario = new Usuario
        {
            Id = idUsuario,
            Mail = "hardcode@mail.com",
            NombreUsuario = "HardCodeUser123",
            HashDeContrasena = "abc123"
        };

        return this.repositorioPartida.CrearPartida(usuario);
    }

    public List<PartidaDTO> ObtenerPartidas()
    {
            var partidas = this.repositorioPartida.ObtenerPartidas();
            return partidas
              .Select(p => this.PartidaToDTO(p))
              .ToList();
    }

    public PartidaDTO ObtenerPorUsuarioId(int IdUsuario)
    {
        Partida partida = this.repositorioPartida.ObtenerPorUsuarioId(IdUsuario);
        if (partida == null) throw new Exception("Partida no encontrada");
        return this.PartidaToDTO(partida);
    }

    private PartidaDTO PartidaToDTO (Partida partida)
    {
        return new PartidaDTO
        {
            Id = partida.Id,
            Partida = partida,
        };
    }
}

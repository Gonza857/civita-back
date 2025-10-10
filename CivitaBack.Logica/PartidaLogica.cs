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
    PartidaDTO CrearPartida();

    List<PartidaDTO> ObtenerPartidas();
}

public class PartidaLogica : IPartidaLogica
{

    private readonly IRepositorioPartida repositorioPartida;
    
    public PartidaLogica(IRepositorioPartida repositoriopartida)
    {
        repositorioPartida = repositoriopartida;
    }

    public PartidaDTO CrearPartida()
    {
        Usuario usuario = new Usuario
        {
            Mail = "hardcode@mail.com",
            NombreUsuario = "HardCodeUser123",
            HashDeContrasena = "abc123"
        };
        Partida creada = this.repositorioPartida.CrearPartida(usuario);
        return new PartidaDTO 
        {
            Id = creada.Id,
            Partida = creada,
        };
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

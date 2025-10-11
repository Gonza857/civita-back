using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.EF;
using CivitaBack.Data.Enum;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;

namespace CivitaBack.Logica;

public interface IPartidaLogica
{
    PartidaDTO ObtenerPorUsuarioId(int IdUsuario);
    Partida CrearPartida(int idUsuario);

    void Actualizar(PartidaDTO partida, Usuario usuario);
    List<PartidaDTO> ObtenerPartidas();
}

public class PartidaLogica : IPartidaLogica
{

    private readonly IRepositorioPartida repositorioPartida;
    
    public PartidaLogica(IRepositorioPartida repositoriopartida)
    {
        repositorioPartida = repositoriopartida;
    }

    public void Actualizar(PartidaDTO partida, Usuario usuario)
    {
        if (partida == null && usuario == null) throw new ErrorInternoExcepction("Ocurrió un error al actualizar la Partida");
        var partidaBuscada = this.repositorioPartida.ObtenerPorUsuarioId(usuario.Id);
        if (partidaBuscada == null) throw new ErrorInternoExcepction("Ocurrió un error al actualizar la Partida");

        // Actualizar recursos de forma segura
        var energia = partidaBuscada.Recursos
            .FirstOrDefault(r => r.Nombre == TipoRecurso.Energia.GetDescription());
        if (energia != null) energia.Cantidad = partida.Energia;

        var felicidad = partidaBuscada.Recursos
            .FirstOrDefault(r => r.Nombre == TipoRecurso.Felicidad.GetDescription());
        if (felicidad != null) felicidad.Cantidad = partida.Felicidad;

        var ecoCoins = partidaBuscada.Recursos
            .FirstOrDefault(r => r.Nombre == TipoRecurso.EcoCoins.GetDescription());
        if (ecoCoins != null) ecoCoins.Cantidad = partida.EcoCoins;

        var contaminacion = partidaBuscada.Recursos
            .FirstOrDefault(r => r.Nombre == TipoRecurso.Contaminacion.GetDescription());
        if (contaminacion != null) contaminacion.Cantidad = partida.Contaminacion;

        this.repositorioPartida.Actualizar();

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
        PartidaDTO partidaDTO = new PartidaDTO
        {
            Id = partida.Id,
            Partida = partida,
            UsuarioId = partida.Usuario.Id,
            Usuario = partida.Usuario.NombreUsuario
            
        };

        partidaDTO.Energia = partida.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Energia.GetDescription())?.Cantidad ?? 0;
        partidaDTO.Felicidad = partida.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Felicidad.GetDescription())?.Cantidad ?? 0;
        partidaDTO.EcoCoins = partida.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.EcoCoins.GetDescription())?.Cantidad ?? 0;
        partidaDTO.Contaminacion = partida.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Contaminacion.GetDescription())?.Cantidad ?? 0;

        return partidaDTO;
    }
}

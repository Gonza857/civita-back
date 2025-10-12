using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.EF;
using CivitaBack.Data.Repositorio;

namespace CivitaBack.Logica;

public interface IPartidaLogica
{
    Partida ObtenerPorUsuarioId(int IdUsuario);
    PartidaDTO CrearPartida();
    List<PartidaDTO> ObtenerPartidas();

    Task GuardarMapaAsync(GuardarMapaDTO dto);

    Task<GuardarMapaDTO?> ObtenerMapaAsync(int partidaId);

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

    public Partida ObtenerPorUsuarioId(int IdUsuario)
    {
        throw new NotImplementedException();
    }

    private PartidaDTO PartidaToDTO(Partida partida)
    {
        return new PartidaDTO
        {
            Id = partida.Id,
            Partida = partida,
        };
    }

 
    public async Task GuardarMapaAsync(GuardarMapaDTO dto)
    {
        if (dto == null || dto.PartidaId <= 0)
            throw new ArgumentException("Datos inválidos para guardar el mapa.");

        //Actualiza el JSON del mapa
        await repositorioPartida.ActualizarMapaAsync(dto.PartidaId, dto.JsonMapa);

        //Si vienen estructuras, sincronizarlas
        if (dto.Estructuras != null && dto.Estructuras.Any())
        {
            await repositorioPartida.ActualizarEstructurasMapaAsync(dto.PartidaId, dto.Estructuras);
        }
    }

    public async Task<GuardarMapaDTO?> ObtenerMapaAsync(int partidaId)
    {
        return await repositorioPartida.ObtenerMapaAsync(partidaId);
    }



}

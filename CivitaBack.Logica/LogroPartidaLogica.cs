using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface ILogroPartidaLogica
{
    Task<List<LogroDTO>> ObtenerLogrosIncompletos(int usuarioId);
    Task<List<LogroDTO>> ObtenerLogrosCompletados(int usuarioId);

    Task ReiniciarLogros(int partidaId);

}

public class LogroPartidaLogica : ILogroPartidaLogica, IParser<Logro, LogroDTO>
{
    private readonly ILogroPartidaRepositorio repositorioLogroPartida;
    private readonly IPartidaRepositorio repositorioPartida;

    public LogroPartidaLogica(ILogroPartidaRepositorio rlp, IPartidaRepositorio pr)
    {
        repositorioLogroPartida = rlp;
        repositorioPartida = pr;
    }

    public async Task<List<LogroDTO>> ObtenerLogrosIncompletos(int usuarioId)
    {
        if (usuarioId <= 0)
            throw new LogroPartidaExcepcion("Ocurrió un error al obtener los logros del usuario.");
        Partida partida = await this.ObtenerPartidaUsuarioPorId(usuarioId);
        List<Logro> logrosNoCompletos = await repositorioLogroPartida.ObtenerLogrosIncompletos(partida.Id);
        return logrosNoCompletos.Select(l => this.ToDto(l)).ToList();
    }

    public async Task<List<LogroDTO>> ObtenerLogrosCompletados(int usuarioId)
    {
        if (usuarioId <= 0)
            throw new LogroPartidaExcepcion("Ocurrió un error al obtener los logros del usuario.");

        Partida partida = await this.ObtenerPartidaUsuarioPorId(usuarioId);
        List<Logro> logrosCompletados = await repositorioLogroPartida.ObtenerLogrosCompletos(partida.Id);
        return logrosCompletados.Select(l => this.ToDto(l)).ToList();

    }

    private async Task<Partida> ObtenerPartidaUsuarioPorId(int usuarioId)
    {
        Partida? partida = await repositorioPartida.ObtenerPorUsuarioId(usuarioId);
        if (partida == null) 
            throw new LogroExcepcion("No se encontró la partida del usuario");
        return partida;
    }

    public LogroDTO ToDto(Logro entidad)
    {
        return new LogroDTO
        {
            Id = entidad.Id,
            Descripcion = entidad.Descripcion,
            Tipo = entidad.TipoLogro.Nombre,
            Titulo = entidad.Titulo,
            TipoId = entidad.TipoLogro.Id
        };
    }

    public async Task ReiniciarLogros(int partidaId)
    {
        await repositorioLogroPartida.ReiniciarLogrosPartida(partidaId);
    }
}

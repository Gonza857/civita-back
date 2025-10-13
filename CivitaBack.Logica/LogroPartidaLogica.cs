using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface ILogroPartidaLogica
{
    List<LogroDTO> ObtenerLogrosIncompletos(int usuarioId);
    List<LogroDTO> ObtenerLogrosCompletados(int usuarioId);

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

    public List<LogroDTO> ObtenerLogrosIncompletos(int usuarioId)
    {
        Partida partida = repositorioPartida.ObtenerPorUsuarioId(usuarioId);
        if (partida == null) throw new Exception("No se encontró la partida del usuario");
        List<Logro> logrosNoCompletos = repositorioLogroPartida.ObtenerLogrosIncompletos(partida.Id);
        return logrosNoCompletos.Select(l => this.ToDto(l)).ToList();
    }

    public List<LogroDTO> ObtenerLogrosCompletados(int usuarioId)
    {
        Partida partida = repositorioPartida.ObtenerPorUsuarioId(usuarioId);
        if (partida == null) throw new Exception("No se encontró la partida del usuario");
        List<Logro> logrosNoCompletos = repositorioLogroPartida.ObtenerLogrosCompletos(partida.Id);
        return logrosNoCompletos.Select(l => this.ToDto(l)).ToList();

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
}

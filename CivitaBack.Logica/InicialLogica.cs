using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;
using Microsoft.Extensions.Configuration;

namespace CivitaBack.Logica;

public class InicialLogica : IInicialLogica
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly IPartidaRepositorio _partidaRepositorio;
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public InicialLogica(IUnidadDeTrabajo uow, IPartidaRepositorio partidaRepositorio, IUsuarioRepositorio usuarioRepositorio)
    {
        _uow = uow;
        _usuarioRepositorio = usuarioRepositorio;
        _partidaRepositorio = partidaRepositorio;
    }

    public async Task IniciarPartida(Usuario usuario)
    {
        var partida = new Partida
        {
            Usuario = usuario,
            JsonMapa = this.GenerarMapa(),
            UltimaVez = DateTime.UtcNow,
        };
        partida.Recursos = this.GenerarRecursos(partida);
        await this._partidaRepositorio.Agregar(partida);
        await this._uow.CommitAsync();
    }

    private Recurso GenerarRecursos(Partida partida)
    {
        return new Recurso
        {
            EcoCoins = 450,
            Felicidad = 40,
            Energia = 30,
            Contaminacion = 60,
            Partida = partida,
        };
    }

    private string GenerarMapa()
    {
        var rutaMapa = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..", "..", "..", "..",
            "CivitaBack.Data", "Mapa", "mapa3.json"
        );

        rutaMapa = Path.GetFullPath(rutaMapa);

        if (!File.Exists(rutaMapa))
            throw new FileNotFoundException("No se encontró el archivo de mapa base.", rutaMapa);

        return File.ReadAllText(rutaMapa);
    }
}
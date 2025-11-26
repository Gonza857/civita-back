using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Microsoft.Extensions.Configuration;

namespace CivitaBack.Logica;

public class InicialLogica : IInicialLogica
{
    private readonly IUnidadDeTrabajo _uow;
    private readonly IPartidaRepositorio _partidaRepositorio;
    private readonly IMisionPartidaRepositorio _misionPartidaRepositorio;
    private readonly IMisionRepositorio _misionRepositorio;
    private readonly IAccesoUsuarios _accesoUsuarios;

    public InicialLogica(
        IUnidadDeTrabajo uow, 
        IPartidaRepositorio partidaRepositorio, 
        IMisionPartidaRepositorio misionPartidaRepositorio,
        IUsuarioRepositorio usuarioRepositorio,
        IMisionRepositorio misionRepositorio,
        IAccesoUsuarios accesoUsuarios
        )
    {
        _uow = uow;
        _misionPartidaRepositorio = misionPartidaRepositorio;
        _partidaRepositorio = partidaRepositorio;
        _misionRepositorio = misionRepositorio;
        _accesoUsuarios = accesoUsuarios;
    }

    public async Task IniciarPartida(Usuario usuario)
    {
        // _accesoUsuarios.ValidarAcceso(usuario.Id);

        var misiones = await this._misionRepositorio.ObtenerTodos();
        
        var partida = new Partida
        {
            Usuario = usuario,
            JsonMapa = this.GenerarMapa(),
            UltimaVez = DateTime.UtcNow,
            Nivel = 0,
            Experiencia = 0,
            Recursos = this.GenerarRecursos(),
        };
        
        partida.MisionPartidas = this._misionPartidaRepositorio.AgregarMisionesPartida(misiones, partida);
        
        await this._partidaRepositorio.Agregar(partida);
        await this._uow.CommitAsync();
    }

    private Recurso GenerarRecursos()
    {
        return new Recurso
        {
            EcoCoins = 450,
            Felicidad = 50,
            Energia = 40,
            Contaminacion = 40,
        };
    }

    private string GenerarMapa()
    {
        string baseDirectorio = AppContext.BaseDirectory;
        var rutaMapa = Path.Combine(baseDirectorio, "Mapa", "MapaJuego.json");
        
        if (!File.Exists(rutaMapa))
        {
            // _logger.LogError("El archivo de mapa no se encuentra en la ruta esperada: {rutaMapa}", rutaMapa);
            throw new FileNotFoundException("No se encontró el archivo de mapa base.", rutaMapa);
        }

        return File.ReadAllText(rutaMapa);
    }
}
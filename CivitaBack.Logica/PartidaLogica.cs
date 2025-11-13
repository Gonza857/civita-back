using CivitaBack.Data.Repositorio;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class PartidaLogica : IPartidaLogica
{
    private readonly IPartidaRepositorio _repositorioPartida;
    private readonly IRecursoRepositorio _recursoRepositorio;
    private readonly IEstructuraMapaRepositorio _repositorioEstructuraMapa;
    private readonly ILogroRepositorio _logroRepositorio;
    private readonly IEstructuraRepositorio _estructuraRepositorio;
    private readonly IUnidadDeTrabajo _uow;
    
    public PartidaLogica(
        IPartidaRepositorio rp, 
        IRecursoRepositorio irr, 
        IEstructuraMapaRepositorio em, 
        ILogroRepositorio ilr,
        IEstructuraRepositorio er,
        IUnidadDeTrabajo uow
        )
    {
        this._repositorioPartida = rp;
        this._recursoRepositorio = irr;
        this._repositorioEstructuraMapa = em;
        this._logroRepositorio = ilr;
        this._estructuraRepositorio = er;
        this._uow = uow;
    }

    /// <summary>
    /// Valida los recursos entrantes de una partida para luego ser actualizados.
    /// </summary>
    /// <param name="partida">PartidaDTO</param>
    private void ValidarRecursosPartida(Partida? partida)
    {
        if (partida == null || partida.Recursos == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");

        var recursosPartida = partida.Recursos;
        
        if (recursosPartida.Energia < 0 || recursosPartida.Felicidad < 0 ||
            recursosPartida.EcoCoins < 0 || recursosPartida.Contaminacion < 0)
            throw new PartidaExcepcion("Los valores de los recursos no pueden ser negativos");
    }

    /// <summary>
    /// Actualizar la partida y los recursos
    /// </summary>
    /// <param name="partida">PartidaDTO</param>
    /// <param name="usuario">Usuario</param>
    public async Task Actualizar(Partida partida, Usuario usuario)
    {
        this.ValidarRecursosPartida(partida);
        if (usuario == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");
        
        Partida? partidaBuscada = await this._repositorioPartida.ObtenerPorUsuarioId(usuario.Id);
        
        if (partidaBuscada == null || partidaBuscada.Recursos == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: No encontrada.");
        
        partidaBuscada!.Recursos.Contaminacion = partida.Recursos!.Contaminacion;
        partidaBuscada.Recursos.Energia = partida.Recursos.Energia;
        partidaBuscada.Recursos.Felicidad = partida.Recursos.Felicidad;
        partidaBuscada.Recursos.EcoCoins = partida.Recursos.EcoCoins;
        
        try
        {
            await this._repositorioPartida.Actualizar(partidaBuscada);
            await this._uow.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new ErrorInternoException("Ocurrió un error al Actualizar un la Partida");
        }
    }
    
    /// <summary>
    /// Crea una partida nueva para un usuario
    /// </summary>
    /// <param name="idUsuario">Id de usuario</param>
    public async Task<Partida> CrearPartida(int idUsuario)
    {
        var partidaExistente = await this._repositorioPartida.ObtenerPorUsuarioId(idUsuario);
        if (partidaExistente != null)
            throw new PartidaExcepcion("Ya tienes una partida empezada.");
        if (idUsuario <= 0)
            throw new PartidaExcepcion("El Id del usuario es inválido.");
        
        try
        {
            var partida = await this._repositorioPartida.CrearPartida(idUsuario);
            await this._uow.CommitAsync();
            return partida;
        }
        catch (Exception ex)
        {
            throw new ErrorInternoException("Ocurrió un error al Actualizar un la Partida");
        }
        
    }

    // 📜 OBTENER TODAS LAS PARTIDAS
    public async Task<List<Partida>> ObtenerPartidas()
    {
        return await this._repositorioPartida.ObtenerTodos();
    }

    public async Task<Partida?> ObtenerPartidaPorIdInterno(int idUsuario)
    {
        return await this._repositorioPartida.ObtenerPorUsuarioId(idUsuario);
    }

    /// <summary>
    /// Devuelve la partida de un usuario
    /// </summary>
    /// <param name="idUsuario">ID del Usuario</param>
    public async Task<Partida> ObtenerPorUsuarioId(int IdUsuario)
    {
        var partida = await this._repositorioPartida.ObtenerPorUsuarioId(IdUsuario);
        if (partida == null) throw new PartidaExcepcion("Partida no encontrada");
        return partida;
    }
    
    public async Task ReclamarLogros(Partida partida, List<Logro> logrosDto)
    {
        if (partida == null) throw new PartidaExcepcion("Ocurrió un error al reclamar los logros");

        var logrosDb = await this._logroRepositorio.ObtenerTodos();

        var logrosCoincidentes = logrosDb
            .Where(l => logrosDto.Any(dto => dto.Id == l.Id))
            .ToList();

        List<Condicion> recompensas = logrosCoincidentes
            .Where(l => l.Condicion?.Recompensa != null)
            .Select(l => l.Condicion!.Recompensa!)
            .ToList();

        Recurso? recursoPartida = await this._recursoRepositorio.ObtenerPorId(partida.Id);
        if (recursoPartida == null)
            throw new PartidaExcepcion("Recursos de partida no encontrados");
        
        // 🔥 Aplica cada recompensa sobre el recurso usando reflexión
        foreach (var recompensa in recompensas)
        {
            if (!string.IsNullOrEmpty(recompensa.NombreColumna))
            {
                var propiedad = typeof(Recurso).GetProperty(recompensa.NombreColumna!);

                if (propiedad != null && propiedad.PropertyType == typeof(int))
                {
                    int valorActual = (int)propiedad.GetValue(recursoPartida)!;
                    propiedad.SetValue(recursoPartida, valorActual + recompensa.Cantidad);
                }
                else
                {
                    Console.WriteLine($"⚠️ Propiedad {recompensa.NombreColumna} no encontrada o no es int en Recurso");
                }
            }
        }

        await this._recursoRepositorio.Actualizar(recursoPartida);

        await this._uow.CommitAsync();
    }
    
    public async Task<Partida> ObtenerPorId(int idPartida)
    {
        var partida = await this._repositorioPartida.ObtenerPorId(idPartida);
        if (partida == null)
            throw new PartidaExcepcion("Partida no encontrada");
        return partida;
    }

    public async Task<int> ComprarEstructuraAsync(int partidaId, int estructuraId)
    {
        Partida? partida = await _repositorioPartida.ObtenerPorId(partidaId);
        Estructura? estructura = await _estructuraRepositorio.ObtenerPorId(estructuraId);

        if (partida == null || partida.Recursos == null)
            throw new PartidaExcepcion("Partida inválida o recursos no encontrados.");
        if (estructura == null)
            throw new PartidaExcepcion("Estructura no encontrada.");

        int costo = estructura.CostoDinero;

        if (partida.Recursos.EcoCoins < costo)
            throw new PartidaExcepcion("Dinero insuficiente.");

        partida.Recursos.EcoCoins -= costo;

        await _repositorioPartida.Actualizar(partida);

        await _uow.CommitAsync();

        return partida.Recursos.EcoCoins;
    }
}


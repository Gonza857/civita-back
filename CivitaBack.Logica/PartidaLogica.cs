using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class PartidaLogica : IPartidaLogica
{
    private readonly IPartidaRepositorio _repositorioPartida;
    private readonly IRecursoRepositorio _recursoRepositorio;
    private readonly IEstructuraMapaRepositorio _repositorioEstructuraMapa;
    private readonly ILogroRepositorio _logroRepositorio;
    private readonly IUnidadDeTrabajo _uow;
    
    public PartidaLogica(
        IPartidaRepositorio rp, 
        IRecursoRepositorio irr, 
        IEstructuraMapaRepositorio em, 
        ILogroRepositorio ilr,
        IUnidadDeTrabajo uow
        )
    {
        this._repositorioPartida = rp;
        this._recursoRepositorio = irr;
        this._repositorioEstructuraMapa = em;
        this._logroRepositorio = ilr;
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
            throw new ErrorInternoExcepction("Ocurrió un error al Actualizar un la Partida");
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
            throw new ErrorInternoExcepction("Ocurrió un error al Actualizar un la Partida");
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

    /// <summary>
    /// Guarda el mapa de la partida. Si tiene estructuras, las actualiza.
    /// </summary>
    /// <param name="dto">GuardarMapaDTO</param>
    public async Task ActualizarMapaDePartidaAsync(int partidaId, string jsonMapa, List<EstructuraMapa>? estructuras)
    {
        if (jsonMapa == null || partidaId <= 0 ||  estructuras == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");

        var partida = await _repositorioPartida.ObtenerPartidaConMapaAsync(partidaId);
        if (partida == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: No existe la partida.");

        partida.JsonMapa = jsonMapa;
        partida.UltimaVez = DateTime.UtcNow;
        await _repositorioPartida.ActualizarMapaAsync(partida);

        if (estructuras.Any())
        {
            // 1️⃣ Eliminar estructuras viejas de esa partida
            await _repositorioEstructuraMapa.EliminarPorPartidaIdAsync(partida.Id);

            // 2️⃣ Agregar las nuevas
            var nuevas = estructuras.Select(e => new EstructuraMapa
            {
                PartidaId = partida.Id,
                EstructuraId = e.EstructuraId,
                X = e.X,
                Y = e.Y,
                Width = e.Width,
                Height = e.Height
            }).ToList();

            await _repositorioEstructuraMapa.AgregarNuevas(nuevas);

            // 3️⃣ Guardar cambios
            await this._uow.CommitAsync();
        }
        else
        {
            await this._uow.CommitAsync();
        }
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
    

    /// <summary>
    /// Obtiene el mapa de una partida.
    /// </summary>  
    /// <param name="partidaId">ID de partida</param>
    public async Task<Partida?> ObtenerMapaAsync(int partidaId)
    {
        var partida = await _repositorioPartida.ObtenerPartidaConMapaAsync(partidaId);
        if (partida == null) return null;

        if (!string.IsNullOrWhiteSpace(partida.JsonMapa)) return partida;

        var mapaReconstruido = await _repositorioPartida.ObtenerMapaJsonPorPartidaIdAsync(partidaId);
        partida.JsonMapa = mapaReconstruido;

        await _repositorioPartida.ActualizarMapaAsync(partida);

        await _uow.CommitAsync();

        return partida;
    }

    
}


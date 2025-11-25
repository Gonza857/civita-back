using CivitaBack.Data.Repositorio;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public class PartidaLogica : IPartidaLogica
{
    private readonly IPartidaRepositorio _repositorioPartida;
    private readonly IRecursoRepositorio _recursoRepositorio;
    private readonly ILogroRepositorio _logroRepositorio;
    private readonly IAccesoUsuarios _accesoUsuarios;
    private readonly IUnidadDeTrabajo _uow;
    
    public PartidaLogica(
        IPartidaRepositorio rp, 
        IRecursoRepositorio irr, 
        ILogroRepositorio ilr,
        IAccesoUsuarios accesoUsuarios,
        IUnidadDeTrabajo uow
        )
    {
        this._repositorioPartida = rp;
        this._recursoRepositorio = irr;
        this._logroRepositorio = ilr;
        this._accesoUsuarios = accesoUsuarios;
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

        _accesoUsuarios.ValidarAcceso(usuario.Id);

        Partida? partidaBuscada = await this._repositorioPartida.ObtenerPorUsuarioId(usuario.Id);
        
        if (partidaBuscada == null || partidaBuscada.Recursos == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: No encontrada.");
        
        partidaBuscada.Recursos.Contaminacion = partida.Recursos!.Contaminacion;
        partidaBuscada.Recursos.Energia = partida.Recursos.Energia;
        partidaBuscada.Recursos.Felicidad = partida.Recursos.Felicidad;
        partidaBuscada.Recursos.EcoCoins = partida.Recursos.EcoCoins;
        partidaBuscada.Nivel = partida.Nivel;
        partidaBuscada.Experiencia = partida.Experiencia;
        
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

    public async Task Actualizar(Partida partida)
    {
        this.ValidarRecursosPartida(partida);
        
        Partida? partidaBuscada = await this._repositorioPartida.ObtenerPorId(partida.Id);
        
        if (partidaBuscada == null || partidaBuscada.Recursos == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: No encontrada.");
        
        partidaBuscada.Recursos.Contaminacion = partida.Recursos!.Contaminacion;
        partidaBuscada.Recursos.Energia = partida.Recursos.Energia;
        partidaBuscada.Recursos.Felicidad = partida.Recursos.Felicidad;
        partidaBuscada.Recursos.EcoCoins = partida.Recursos.EcoCoins;
        partidaBuscada.Nivel = partida.Nivel;
        partidaBuscada.Experiencia = partida.Experiencia;
        
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
        this._accesoUsuarios.ValidarAcceso(idUsuario);

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
        if (!_accesoUsuarios.EsDios())
            throw new AccesoDenegadoExcepcion("No tenes permiso para acceder a las partidas.");

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
        _accesoUsuarios.ValidarAcceso(IdUsuario);

        var partida = await this._repositorioPartida.ObtenerPorUsuarioId(IdUsuario);
        if (partida == null) throw new PartidaExcepcion("Partida no encontrada");
        return partida;
    }
    
    public async Task ReclamarLogros(Partida partida, List<Logro> logrosDto)
    {
        if (partida == null) throw new PartidaExcepcion("Ocurrió un error al reclamar los logros");

        _accesoUsuarios.ValidarAcceso(partida.UsuarioId);

        var logrosDb = await this._logroRepositorio.ObtenerTodos();

        var logrosCoincidentes = logrosDb
            .Where(l => logrosDto.Any(dto => dto.Id == l.Id))
            .ToList();

        List<Recompensa> recompensas = logrosCoincidentes
            .SelectMany(l => l.Condicion.Recompensas)
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
        
        // _accesoUsuarios.ValidarAcceso(partida.UsuarioId);

        return partida;
    }

    public async Task<Partida> ObtenerPartidaParaLogin(int idUsuario)
    {
        var partida = await this._repositorioPartida.ObtenerPorUsuarioId(idUsuario);
        if (partida == null) throw new PartidaExcepcion("Partida no encontrada");
        return partida;
    }
}


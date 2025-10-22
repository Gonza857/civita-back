using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Enum;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Logica;


public interface IPartidaLogica
{
    Task<Partida> ObtenerPorUsuarioId(int IdUsuario);
    Task<Partida> CrearPartida(int idUsuario);
    Task Actualizar(PartidaDTO partida, Usuario usuario);
    Task<List<PartidaDTO>> ObtenerPartidas();

    // 🆕 Métodos de mapa
    Task ActualizarMapaDePartidaAsync(GuardarMapaDTO dto);
    
    
    Task<Partida?> ObtenerMapaAsync(int partidaId);
}

public class PartidaLogica : IPartidaLogica
{
    private readonly IPartidaRepositorio _repositorioPartida;
    private readonly IRecursoLogica _recursoLogica;
    private readonly IEstructuraMapaRepositorio _estructuraMapaRepositorio;

    public PartidaLogica(IPartidaRepositorio rp, IRecursoLogica rl, IEstructuraMapaRepositorio em)
    {
        this._repositorioPartida = rp;
        this._recursoLogica = rl;
        this._estructuraMapaRepositorio = em;
    }

    /// <summary>
    /// Valida los recursos entrantes de una partida para luego ser actualizados.
    /// </summary>
    /// <param name="partida">PartidaDTO</param>
    private void ValidarRecursosPartida(PartidaDTO? partida)
    {
        if (partida == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");
        
        if (partida.Energia < 0 || partida.Felicidad < 0 ||
            partida.EcoCoins < 0 || partida.Contaminacion < 0)
            throw new PartidaExcepcion("Los valores de los recursos no pueden ser negativos");
    }

    /// <summary>
    /// Actualizar la partida y los recursos
    /// </summary>
    /// <param name="partida">PartidaDTO</param>
    /// <param name="usuario">Usuario</param>
    public async Task Actualizar(PartidaDTO partida, Usuario usuario)
    {
        this.ValidarRecursosPartida(partida);
        if (usuario == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");
        
        Partida? partidaBuscada = await this._repositorioPartida.ObtenerPorUsuarioId(usuario.Id);
        
        if (partidaBuscada == null || partidaBuscada.Recursos == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: No encontrada.");
        
        partidaBuscada!.Recursos.Contaminacion = partida.Contaminacion;
        partidaBuscada.Recursos.Energia = partida.Energia;
        partidaBuscada.Recursos.Felicidad = partida.Felicidad;
        partidaBuscada.Recursos.EcoCoins = partida.EcoCoins;
        
        await this._repositorioPartida.GuardarCambios();
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
        
        var partida = await this._repositorioPartida.CrearPartida(idUsuario);

        // 🔹 Inicializar recursos para esa partida
        //this.recursoLogica.ConfigurarInicial(partida);

        return partida;
    }

    // 📜 OBTENER TODAS LAS PARTIDAS
    public async Task<List<PartidaDTO>> ObtenerPartidas()
    {
        var partidas = await this._repositorioPartida.ObtenerPartidas();
        return partidas.Select(p => this.PartidaToDTO(p)).ToList();
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
    /// Convierte la entidad de dominio (BO) en un DTO para devolver como respuesta.
    /// </summary>
    /// <param name="partida">Partida con recursos</param>
    private PartidaDTO PartidaToDTO(Partida partida)
    {
        return new PartidaDTO
        {
            Id = partida.Id,
            Partida = partida,
            UsuarioId = partida.Usuario?.Id ?? 0,
            Usuario = partida.Usuario?.NombreUsuario ?? string.Empty,
            Contaminacion = partida.Recursos.Contaminacion,
            Felicidad = partida.Recursos.Felicidad,
            EcoCoins = partida.Recursos.EcoCoins,
            Energia = partida.Recursos.Energia,
        };
    }

    /// <summary>
    /// Guarda el mapa de la partida. Si tiene estructuras, las actualiza.
    /// </summary>
    /// <param name="dto">GuardarMapaDTO</param>
    public async Task ActualizarMapaDePartidaAsync(GuardarMapaDTO dto)
    {
        if (dto == null || dto.PartidaId <= 0)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");

        var partida = await _repositorioPartida.ObtenerPartidaConMapaAsync(dto.PartidaId);
        if (partida == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: No existe la partida.");

        partida.JsonMapa = dto.JsonMapa;
        partida.UltimaVez = DateTime.UtcNow;
        await _repositorioPartida.ActualizarMapaAsync(partida);

        if (dto.Estructuras != null && dto.Estructuras.Any())
        {
            // 1️⃣ Eliminar estructuras viejas de esa partida
            await _estructuraMapaRepositorio.EliminarPorPartidaIdAsync(partida.Id);

            // 2️⃣ Agregar las nuevas
            var nuevas = dto.Estructuras.Select(e => new EstructuraMapa
            {
                PartidaId = partida.Id,
                EstructuraId = e.EstructuraId,
                X = e.X,
                Y = e.Y,
                Width = e.Width,
                Height = e.Height
            }).ToList();

            _estructuraMapaRepositorio.AgregarNuevas(nuevas);

            // 3️⃣ Guardar cambios
            await _estructuraMapaRepositorio.GuardarCambios();
        }
    }


    private void ActualizarEstructuraMapa(EstructuraMapa original, EstructuraMapaDTO mod)
    {
        original.EstructuraId = mod.EstructuraId;
        original.X = mod.X;
        original.Y = mod.Y;
        original.Width = mod.Width;
        original.Height = mod.Height;
    }

    private List<EstructuraMapa> ListaDtoToListaEntidad(List<EstructuraMapaDTO> emDtoList, int idPartida)
    {
        var lista = new List<EstructuraMapa>();
        foreach (var emDto in emDtoList)
        {
            lista.Add(new EstructuraMapa
            {
                EstructuraId = emDto.EstructuraId,
                X = emDto.X,
                Y = emDto.Y,
                Width = emDto.Width,
                Height = emDto.Height,
                PartidaId = idPartida,
            });
        }
        return lista;
    }

    /// <summary>
    /// Obtiene el mapa de una partida.
    /// </summary>  
    /// <param name="partidaId">ID de partida</param>
    public async Task<Partida?> ObtenerMapaAsync(int partidaId)
    {
        var partida = await _repositorioPartida.ObtenerPartidaConMapaAsync(partidaId);
        if (partida == null)
            return null;

        if (!string.IsNullOrWhiteSpace(partida.JsonMapa))
            return partida;

        var mapaReconstruido = await _repositorioPartida.ObtenerMapaJsonPorPartidaIdAsync(partidaId);
        partida.JsonMapa = mapaReconstruido;

        await _repositorioPartida.ActualizarMapaAsync(partida);

        return partida;
    }

    
}


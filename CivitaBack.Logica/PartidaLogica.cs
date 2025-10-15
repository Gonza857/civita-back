using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Enum;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;

namespace CivitaBack.Logica;


public interface IPartidaLogica
{
    PartidaDTO ObtenerPorUsuarioId(int IdUsuario);
    Partida CrearPartida(int idUsuario);
    Task Actualizar(PartidaDTO partida, Usuario usuario);
    List<PartidaDTO> ObtenerPartidas();

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
    /// Actualizar la partida y los recursos
    /// </summary>
    /// <param name="partida">PartidaDTO</param>
    /// <param name="usuario">Usuario</param>
    public async Task Actualizar(PartidaDTO partida, Usuario usuario)
    {
        if (partida == null && usuario == null)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");

        if (partida.Energia < 0 || partida.Felicidad < 0 ||
            partida.EcoCoins < 0 || partida.Contaminacion < 0)
            throw new PartidaExcepcion("Los valores de los recursos no pueden ser negativos");

        var partidaBuscada = this._repositorioPartida.ObtenerPorUsuarioId(usuario.Id);
        
        if (partidaBuscada == null)
            throw new ErrorInternoExcepction("Ocurrió un error al guardar el mapa: No encontrada.");

        var energia = partidaBuscada.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Energia.GetDescription());
        if (energia != null) energia.Cantidad = partida.Energia;

        var felicidad = partidaBuscada.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Felicidad.GetDescription());
        if (felicidad != null) felicidad.Cantidad = partida.Felicidad;

        var ecoCoins = partidaBuscada.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.EcoCoins.GetDescription());
        if (ecoCoins != null) ecoCoins.Cantidad = partida.EcoCoins;

        var contaminacion = partidaBuscada.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Contaminacion.GetDescription());
        if (contaminacion != null) contaminacion.Cantidad = partida.Contaminacion;

        await this._repositorioPartida.GuardarCambios();
    }

    // 🧱 CREAR PARTIDA
    public Partida CrearPartida(int idUsuario)
    {
        var partidaExistente = this._repositorioPartida.ObtenerPorUsuarioId(idUsuario);
        if (partidaExistente != null)
            throw new PartidaExcepcion("Ya tienes una partida empezada.");

        if (idUsuario <= 0)
            throw new PartidaExcepcion("El Id del usuario es inválido.");

        // 🔹 Crear partida en la BD
        var partida = this._repositorioPartida.CrearPartida(idUsuario);

        // 🔹 Inicializar recursos para esa partida
        //this.recursoLogica.ConfigurarInicial(partida);

        return partida;
    }

    // 📜 OBTENER TODAS LAS PARTIDAS
    public List<PartidaDTO> ObtenerPartidas()
    {
        var partidas = this._repositorioPartida.ObtenerPartidas();
        return partidas.Select(p => this.PartidaToDTO(p)).ToList();
    }

    // 🔎 OBTENER PARTIDA POR USUARIO
    public PartidaDTO ObtenerPorUsuarioId(int IdUsuario)
    {
        var partida = this._repositorioPartida.ObtenerPorUsuarioId(IdUsuario);
        if (partida == null) throw new Exception("Partida no encontrada");
        return this.PartidaToDTO(partida);
    }

    // 🔄 CONVERSOR
    private PartidaDTO PartidaToDTO(Partida partida)
    {
        var partidaDTO = new PartidaDTO
        {
            Id = partida.Id,
            Partida = partida,
            UsuarioId = partida.Usuario?.Id ?? 0,
            Usuario = partida.Usuario?.NombreUsuario ?? string.Empty,
        };

        partidaDTO.Energia = partida.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Energia.GetDescription())?.Cantidad ?? 100;
        partidaDTO.Felicidad = partida.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Felicidad.GetDescription())?.Cantidad ?? 50;
        partidaDTO.EcoCoins = partida.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.EcoCoins.GetDescription())?.Cantidad ?? 200;
        partidaDTO.Contaminacion = partida.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Contaminacion.GetDescription())?.Cantidad ?? 60;

        return partidaDTO;
    }

    /// <summary>
    /// Guarda el mapa de la partida. Si tiene estructuras, las actualiza.
    /// </summary>
    /// <param name="dto">GuardarMapaDTO</param>
    public async Task ActualizarMapaDePartidaAsync(GuardarMapaDTO dto)
    {
        if (dto == null || dto.PartidaId <= 0)
            throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: Datos inválidos.");

        var hayEstructurasParaActualizar = dto.Estructuras != null && dto.Estructuras.Any();

        Partida partida = await this._repositorioPartida.ObtenerPartidaConMapaAsync(dto.PartidaId);
        if (partida == null) throw new PartidaExcepcion("Ocurrió un error al guardar el mapa: No existe.");

        partida.JsonMapa = dto.JsonMapa;
        partida.UltimaVez = DateTime.UtcNow;

        await this._repositorioPartida.ActualizarMapaAsync(partida);
        
        if (hayEstructurasParaActualizar)
        {
            List<EstructuraMapa> estructurasDePartida =
                await this._repositorioPartida.ObtenerEstructurasDeUnMapa(dto.PartidaId);

            var nuevas = dto.Estructuras!
                .Where(emDto => emDto.EstructuraId == 0 ||
                                !estructurasDePartida.Any(em => em.EstructuraId == emDto.EstructuraId))
                .ToList();

            var modificadas = dto.Estructuras!
                .Where(emDto => estructurasDePartida.Any(em => em.EstructuraId == emDto.EstructuraId))
                .ToList();

            var eliminadas = estructurasDePartida
                .Where(em => !dto.Estructuras!.Any(emDto => emDto.EstructuraId == em.EstructuraId))
                .ToList();
        
            this._estructuraMapaRepositorio.AgregarNuevas(this.ListaDtoToListaEntidad(nuevas, partida.Id));

            foreach (EstructuraMapaDTO mod in modificadas)
            {
                var original = estructurasDePartida.First(em => em.EstructuraId == mod.EstructuraId);
                this.ActualizarEstructuraMapa(original, mod);
                this._estructuraMapaRepositorio.AgregarUnica(original);
            }

            this._estructuraMapaRepositorio.RemoverEliminadas(eliminadas);

            try
            {
                await this._repositorioPartida.GuardarCambios();
            }
            catch (Exception e)
            {
                throw new ErrorInternoExcepction("Ocurrió un error al actualizar el mapa y las estructuras");
            }
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


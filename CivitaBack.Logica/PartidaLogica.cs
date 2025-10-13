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
    void Actualizar(PartidaDTO partida, Usuario usuario);
    List<PartidaDTO> ObtenerPartidas();

    // 🆕 Métodos de mapa
    Task GuardarMapaAsync(GuardarMapaDTO dto);
    Task<Partida?> ObtenerMapaAsync(int partidaId);
    Task ActualizarMapaAsync(GuardarMapaDTO dto);
}

    public class PartidaLogica : IPartidaLogica
    {
        private readonly IPartidaRepositorio repositorioPartida;
        private readonly IRecursoLogica recursoLogica;

        public PartidaLogica(IPartidaRepositorio repositorioPartida, IRecursoLogica recursoLogica)
        {
            this.repositorioPartida = repositorioPartida;
            this.recursoLogica = recursoLogica;
        }

        // 🧩 ACTUALIZAR RECURSOS
        public void Actualizar(PartidaDTO partida, Usuario usuario)
        {
            if (partida == null && usuario == null)
                throw new ErrorInternoExcepction("Ocurrió un error al actualizar la Partida");

            if (partida.Energia < 0 || partida.Felicidad < 0 ||
                partida.EcoCoins < 0 || partida.Contaminacion < 0)
                throw new PartidaExcepcion("Los valores de los recursos no pueden ser negativos");

            var partidaBuscada = this.repositorioPartida.ObtenerPorUsuarioId(usuario.Id);
            if (partidaBuscada == null)
                throw new ErrorInternoExcepction("Ocurrió un error al actualizar la Partida");

            var energia = partidaBuscada.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Energia.GetDescription());
            if (energia != null) energia.Cantidad = partida.Energia;

            var felicidad = partidaBuscada.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Felicidad.GetDescription());
            if (felicidad != null) felicidad.Cantidad = partida.Felicidad;

            var ecoCoins = partidaBuscada.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.EcoCoins.GetDescription());
            if (ecoCoins != null) ecoCoins.Cantidad = partida.EcoCoins;

            var contaminacion = partidaBuscada.Recursos.FirstOrDefault(r => r.Nombre == TipoRecurso.Contaminacion.GetDescription());
            if (contaminacion != null) contaminacion.Cantidad = partida.Contaminacion;

            this.repositorioPartida.Actualizar();
        }

    // 🧱 CREAR PARTIDA
    public Partida CrearPartida(int idUsuario)
    {
        var partidaExistente = this.repositorioPartida.ObtenerPorUsuarioId(idUsuario);
        if (partidaExistente != null)
            throw new PartidaExcepcion("Ya tienes una partida empezada.");

        if (idUsuario <= 0)
            throw new PartidaExcepcion("El Id del usuario es inválido.");

        // 🔹 Crear partida en la BD
        var partida = this.repositorioPartida.CrearPartida(idUsuario);

        // 🔹 Inicializar recursos para esa partida
        //this.recursoLogica.ConfigurarInicial(partida);

        return partida;
    }

    // 📜 OBTENER TODAS LAS PARTIDAS
    public List<PartidaDTO> ObtenerPartidas()
        {
            var partidas = this.repositorioPartida.ObtenerPartidas();
            return partidas.Select(p => this.PartidaToDTO(p)).ToList();
        }

        // 🔎 OBTENER PARTIDA POR USUARIO
        public PartidaDTO ObtenerPorUsuarioId(int IdUsuario)
        {
            var partida = this.repositorioPartida.ObtenerPorUsuarioId(IdUsuario);
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

        // 🧠 -------------------------------------------------------------
        // NUEVOS MÉTODOS DE MAPA (JSON + ESTRUCTURAS)
        // -------------------------------------------------------------

        // 💾 Guardar mapa completo
        public async Task GuardarMapaAsync(GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId <= 0)
                throw new ArgumentException("Datos de mapa inválidos");

            await repositorioPartida.ActualizarMapaAsync(dto.PartidaId, dto.JsonMapa);

            if (dto.Estructuras != null && dto.Estructuras.Any())
                await repositorioPartida.ActualizarEstructurasMapaAsync(dto.PartidaId, dto.Estructuras);
        }

        // 🧩 Obtener mapa guardado (JSON o reconstruido)
        public async Task<Partida?> ObtenerMapaAsync(int partidaId)
        {
            var partida = await repositorioPartida.ObtenerPartidaConMapaAsync(partidaId);
            if (partida == null)
                return null;

            if (!string.IsNullOrWhiteSpace(partida.JsonMapa))
                return partida;

            var mapaReconstruido = await repositorioPartida.ObtenerMapaJsonPorPartidaIdAsync(partidaId);
            partida.JsonMapa = mapaReconstruido;

            await repositorioPartida.ActualizarMapaAsync(partidaId, mapaReconstruido);

            return partida;
        }

        // 🔄 Actualizar mapa existente
        public async Task ActualizarMapaAsync(GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId <= 0)
                throw new ArgumentException("Datos inválidos para actualizar mapa");

            await repositorioPartida.ActualizarMapaAsync(dto.PartidaId, dto.JsonMapa);

            if (dto.Estructuras != null && dto.Estructuras.Any())
                await repositorioPartida.ActualizarEstructurasMapaAsync(dto.PartidaId, dto.Estructuras);
        }
    }


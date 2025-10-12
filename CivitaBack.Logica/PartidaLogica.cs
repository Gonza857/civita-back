using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;

namespace CivitaBack.Logica
{
    public interface IPartidaLogica
    {
        Partida ObtenerPorUsuarioId(int IdUsuario);
        PartidaDTO CrearPartida();
        List<PartidaDTO> ObtenerPartidas();

        // 🆕 Métodos nuevos
        Task GuardarMapaAsync(GuardarMapaDTO dto);
        Task<Partida?> ObtenerMapaAsync(int partidaId);
        Task ActualizarMapaAsync(GuardarMapaDTO dto);
    }

    public class PartidaLogica : IPartidaLogica
    {
        private readonly IRepositorioPartida repositorioPartida;

        public PartidaLogica(IRepositorioPartida repositoriopartida)
        {
            repositorioPartida = repositoriopartida;
        }

        public PartidaDTO CrearPartida()
        {
            Usuario usuario = new Usuario
            {
                Mail = "hardcode@mail.com",
                NombreUsuario = "HardCodeUser123",
                HashDeContrasena = "abc123"
            };
            Partida creada = this.repositorioPartida.CrearPartida(usuario);
            return new PartidaDTO
            {
                Id = creada.Id,
                Partida = creada,
            };
        }

        public List<PartidaDTO> ObtenerPartidas()
        {
            var partidas = this.repositorioPartida.ObtenerPartidas();
            return partidas
              .Select(p => this.PartidaToDTO(p))
              .ToList();
        }

        public Partida ObtenerPorUsuarioId(int IdUsuario)
        {
            throw new NotImplementedException();
        }

        private PartidaDTO PartidaToDTO(Partida partida)
        {
            return new PartidaDTO
            {
                Id = partida.Id,
                Partida = partida,
            };
        }

        // 🆕 --------------------------------------------------------------------
        // GUARDAR MAPA COMPLETO (JSON + ESTRUCTURAS)
        // --------------------------------------------------------------------
        public async Task GuardarMapaAsync(GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId <= 0)
                throw new ArgumentException("Datos de mapa inválidos");

            await repositorioPartida.ActualizarMapaAsync(dto.PartidaId, dto.JsonMapa);

            if (dto.Estructuras != null && dto.Estructuras.Any())
                await repositorioPartida.ActualizarEstructurasMapaAsync(dto.PartidaId, dto.Estructuras);
        }

        // 🆕 --------------------------------------------------------------------
        // OBTENER MAPA (desde JSON guardado o reconstruir desde estructuras)
        // --------------------------------------------------------------------
        public async Task<Partida?> ObtenerMapaAsync(int partidaId)
        {
            // Primero intenta traer el mapa directamente desde el JSON
            var partida = await repositorioPartida.ObtenerPartidaConMapaAsync(partidaId);

            if (partida == null)
                return null;

            // Si el mapa ya tiene Json guardado, lo devolvemos tal cual
            if (!string.IsNullOrWhiteSpace(partida.JsonMapa))
                return partida;

            // Si no hay Json, lo reconstruimos con los datos base + estructuras
            var mapaReconstruido = await repositorioPartida.ObtenerMapaJsonPorPartidaIdAsync(partidaId);

            partida.JsonMapa = mapaReconstruido;

            // Opcional: podés actualizar el snapshot automáticamente
            await repositorioPartida.ActualizarMapaAsync(partidaId, mapaReconstruido);

            return partida;
        }

        // 🆕 --------------------------------------------------------------------
        // ACTUALIZAR MAPA EXISTENTE
        // --------------------------------------------------------------------
        public async Task ActualizarMapaAsync(GuardarMapaDTO dto)
        {
            if (dto == null || dto.PartidaId <= 0)
                throw new ArgumentException("Datos inválidos para actualizar mapa");

            await repositorioPartida.ActualizarMapaAsync(dto.PartidaId, dto.JsonMapa);

            if (dto.Estructuras != null && dto.Estructuras.Any())
                await repositorioPartida.ActualizarEstructurasMapaAsync(dto.PartidaId, dto.Estructuras);
        }
    }
}

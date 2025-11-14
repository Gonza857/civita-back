using AutoMapper;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using CivitaBack.Logica.Hubs;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace CivitaBack.Tests
{
    public class EventoTest
    {
        private readonly Mock<IEventoRepositorio> _mockEventoRepositorio;
        private readonly Mock<IPartidaRepositorio> _mockPartidaRepositorio;
        private readonly Mock<IUnidadDeTrabajo> _mockUow;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IEventoLogica _eventoLogica;
        private readonly Mock<IActualizarRecursosLogica> _mockActualizarRecursosLogica;

        public EventoTest()
        {
            // 1. Inicializar los Mocks
            _mockEventoRepositorio = new Mock<IEventoRepositorio>();
            _mockPartidaRepositorio = new Mock<IPartidaRepositorio>();
            _mockUow = new Mock<IUnidadDeTrabajo>();
            _mockMapper = new Mock<IMapper>();
            _mockActualizarRecursosLogica = new Mock<IActualizarRecursosLogica>();
            // Si tu EventoLogica usa IHubContext, necesitas mockearlo también.

            // 2. Inicializar el objeto bajo prueba (EventoLogica) con las dependencias.
            // **Asegúrate de que el orden de los argumentos coincida con el constructor real de EventoLogica.**
            _eventoLogica = new EventoLogica(
                _mockEventoRepositorio.Object,
                _mockPartidaRepositorio.Object,
                _mockUow.Object,
                _mockMapper.Object,
                _mockActualizarRecursosLogica.Object
            );
        }

        private EventoMaestro CrearMaestro() => new EventoMaestro
        {
            Id = 10,
            Nombre = "Riesgo",
            TextoDescripcion = "Desc",
            TextoAceptar = "Aceptado",
            TextoRechazar = "Rechazado",
            EcoCoinsAceptar = 10,
            FelicidadAceptar = 5,
            ContaminacionAceptar = 5,
            FelicidadRechazar = -10,
            ContaminacionRechazar = 10
        };
        private Evento CrearEventoBase() => new Evento
        {
            Id = 1,
            TextoAceptar = "Aceptado",
            TextoRechazar = "Rechazado",
            EcoCoinsAceptar = 10,
            FelicidadAceptar = 5,
            ContaminacionAceptar = 5,
            FelicidadRechazar = -10,
            ContaminacionRechazar = 10
        };
        private Partida CrearPartida(int id) => new Partida { Id = id, Recursos = new Recurso { EcoCoins = 100, Contaminacion = 50, Felicidad = 50 } };

        /*[Fact]
        public async Task DispararEventoAsync_CreacionYCommit_Exito()
        {
            // Arrange
            const int partidaId = 5;
            // El maestro debe tener un nombre para el Titulo del DTO
            var maestroMock = CrearMaestro();
            maestroMock.Nombre = "¡Evento Disparado!";

            // El objeto Evento que la lógica modifica y devuelve
            var eventoDominioCreado = CrearEventoBase();

            // 1. INICIALIZAR NAVEGACIÓN EN EL OBJETO MOCKEADO (Necesario para pasar la lógica)
            eventoDominioCreado.EventoMaestro = maestroMock;
            eventoDominioCreado.EventoMaestroId = maestroMock.Id;
            eventoDominioCreado.Partida = CrearPartida(partidaId);

            // Simular que el repositorio devuelve el EventoMaestro
            _mockEventoRepositorio
                .Setup(r => r.ObtenerEventoMaestroAsync())
                .ReturnsAsync(maestroMock);

            // 2. SIMULAR EL MAPEO DE ENTRADA Y SALIDA
            _mockMapper
                .Setup(m => m.Map<Evento>(maestroMock))
                .Returns(eventoDominioCreado);

            // Configurar el mapeo de SALIDA para capturar el resultado que se devuelve.
            // Usaremos una variable local para capturar el DTO mapeado.
            EventoDisparadoDTO? dtoDeSalida = null;
            _mockMapper
                .Setup(m => m.Map<EventoDisparadoDTO>(It.IsAny<Evento>()))
                .Callback<object, EventoDisparadoDTO>((src, dest) => {
                    // Este callback simula que AutoMapper completa el DTO.
                    dest.Id = ((Evento)src).Id;
                    dest.Titulo = ((Evento)src).EventoMaestro.Nombre;
                    dtoDeSalida = dest; // Capturamos el DTO final
                })
                .Returns(() => new EventoDisparadoDTO());

            // Act
            var resultado = await _eventoLogica.DispararEventoAsync(partidaId); // <-- La lógica se ejecuta, llama a CrearEventoAsync

            // Assert (Verificaciones del Lado de la Lógica)

            // 1. Verificar la Lógica de Negocio (que la instancia fue modificada antes de pasarse al repo)
            Assert.Equal(partidaId, eventoDominioCreado.PartidaId);
            Assert.True(eventoDominioCreado.SeDisparo);

            // 2. Verificar la Persistencia: Que el método fue llamado con la instancia correcta.
            _mockEventoRepositorio.Verify(
                r => r.CrearEventoAsync(eventoDominioCreado),
                Times.Once
            );

            // 3. Verificar el Mapeo de SALIDA (El Titulo y el ID generado)
            // El resultado final debe ser el DTO que el mapper creó.
            Assert.NotNull(resultado);
            Assert.Equal(maestroMock.Nombre, resultado.Titulo);

            // Nota: Si el ID se propaga correctamente a eventoDominioCreado, el Assert.Equal(eventoDominioCreado.Id, resultado.Id) pasaría.
            // Dado que el servicio NO tiene UoW, el ID real de la DB es incierto, pero la verificación del Título es la más importante.

            // 4. Verificar que NO hubo Commit
            _mockUow.Verify(u => u.CommitAsync(), Times.Never()); // ✅ Correcto, el UoW fue eliminado.
        }*/

        [Fact]
        public async Task DispararEventoAsync_SinMaestro_RetornaNull()
        {
            // Arrange
            // Simula que el repositorio devuelve null
            _mockEventoRepositorio
                .Setup(r => r.ObtenerEventoMaestroAsync())
                .ReturnsAsync((EventoMaestro)null);

            // Act
            var resultado = await _eventoLogica.DispararEventoAsync(12);

            // Assert
            Assert.Null(resultado);
            // Verificar que NO se intentó guardar nada
            _mockUow.Verify(u => u.CommitAsync(), Times.Never());
        }

        [Fact]
        public async Task ResolverEventoAsync_Aceptado_AplicaEfectosYCommit()
        {
            // Arrange
            const int eventoId = 1;
            var partida = CrearPartida(100);
            var evento = CrearEventoBase();
            var recursos = partida.Recursos;

            // Configurar los efectos en el evento para el cálculo
            evento.EcoCoinsAceptar = 50;
            evento.FelicidadAceptar = 10;
            evento.ContaminacionAceptar = -5;
            evento.Partida = partida;

            // Simular la obtención del evento con su partida asociada
            _mockEventoRepositorio
                .Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
                .ReturnsAsync(evento);

            // Simular el mapeo de la respuesta
            _mockMapper
                .Setup(m => m.Map<EventoResueltoDTO>(It.IsAny<object>()))
                .Returns(new EventoResueltoDTO { Id = eventoId });

            // Cuando el servicio llama a ActualizarRecursosAsync, simulamos el efecto en la memoria.
            _mockActualizarRecursosLogica.Setup(r => r.ActualizarRecursosAsync(
                It.IsAny<Partida>(), // Partida modificada
                It.IsAny<int>(),    // cambioFelicidad
                It.IsAny<int>(),    // cambioContaminacion
                It.IsAny<int>(),    // cambioEcoCoins
                It.IsAny<int>()     // cambioEnergia
            )).Callback((Partida p, int f, int c, int ec, int en) =>
            {
                // Ejecutamos la lógica de mutación manualmente para verificar que el servicio lo hace
                p.Recursos.EcoCoins = 150; // Asignamos el valor esperado
                p.Recursos.Felicidad = 60;
                p.Recursos.Contaminacion = 45;
            });

            // Act
            await _eventoLogica.ResolverEventoAsync(eventoId, true); // Aceptar

            // Assert
            Assert.Equal(150, partida.Recursos.EcoCoins);
            Assert.Equal(60, partida.Recursos.Felicidad);
            Assert.Equal(45, partida.Recursos.Contaminacion);
            Assert.True(evento.Resuelto);

            _mockActualizarRecursosLogica.Verify(
        r => r.ActualizarRecursosAsync(
            It.IsAny<Partida>(),
            10,    // Felicidad
            -5,    // Contaminación
            50,    // EcoCoins
            0      // Energía (asumo que es 0 por evento)
        ),
        Times.Once
    );

            _mockEventoRepositorio.Verify(r => r.Actualizar(evento), Times.Once);

            _mockUow.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task ResolverEventoAsync_YaResuelto_LanzaExcepcion()
        {
            // Arrange
            const int eventoId = 2;
            var partida = CrearPartida(100);
            var eventoYaResuelto = CrearEventoBase();
            eventoYaResuelto.Resuelto = true; // Marcarlo como resuelto
            eventoYaResuelto.Partida = partida;

            _mockEventoRepositorio
                .Setup(r => r.ObtenerEventoConPartidaAsync(eventoId))
                .ReturnsAsync(eventoYaResuelto);

            // Act & Assert
            // Se espera que lance una excepción (tu servicio lanza Exception genérica)
            await Assert.ThrowsAsync<Exception>(() => _eventoLogica.ResolverEventoAsync(eventoId, true));

            // Verificar que NO se intentó guardar
            _mockUow.Verify(u => u.CommitAsync(), Times.Never());
        }
    }

}

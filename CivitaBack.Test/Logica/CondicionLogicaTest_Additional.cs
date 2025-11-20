// using CivitaBack.Domain.Entidades;
// using CivitaBack.Domain.Enum;
// using CivitaBack.Domain.Excepciones;
// using CivitaBack.Domain.Interfaces.Logica;
// using CivitaBack.Domain.Interfaces.Repositorios;
// using CivitaBack.Logica;
// using CivitaBack.Utils;
// using Moq;
//
// namespace CivitaBack.Tests;
//
// public class CondicionLogicaTest_Additional
// {
//     private readonly Mock<ICondicionRepositorio> _mockCondicionRepositorio;
//     private readonly Mock<IEstructuraRepositorio> _mockEstructuraRepositorio;
//     private readonly Mock<IUnidadDeTrabajo> _mockUow;
//     private readonly ICondicionLogica _condicionLogica;
//
//     public CondicionLogicaTest_Additional()
//     {
//         _mockCondicionRepositorio = new Mock<ICondicionRepositorio>();
//         _mockEstructuraRepositorio = new Mock<IEstructuraRepositorio>();
//         _mockUow = new Mock<IUnidadDeTrabajo>();
//
//         _condicionLogica = new CondicionLogica(
//             _mockCondicionRepositorio.Object,
//             _mockEstructuraRepositorio.Object,
//             _mockUow.Object
//         );
//
//         _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
//     }
//
//     [Fact]
//     public async Task Actualizar_ConRecompensaId_AsociaRecompensa()
//     {
//         // Arrange
//         const int id = 1;
//         const int recompensaId = 2;
//         var condicionExistente = new Condicion
//         {
//             Id = id,
//             Cantidad = 50,
//             NombreColumna = "EcoCoins"
//         };
//         var recompensa = new Condicion
//         {
//             Id = recompensaId,
//             EsRecompensa = true,
//             NombreColumna = "EcoCoins",
//             Cantidad = 10
//         };
//         var condicionActualizada = new Condicion
//         {
//             Cantidad = 100,
//             NombreColumna = "EcoCoins",
//             RecompensaId = recompensaId
//         };
//
//         _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(id))
//             .ReturnsAsync(condicionExistente);
//         _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(recompensaId))
//             .ReturnsAsync(recompensa);
//         _mockCondicionRepositorio.Setup(r => r.Actualizar(It.IsAny<Condicion>()))
//             .Returns(Task.CompletedTask);
//
//         // Act
//         await _condicionLogica.Actualizar(condicionActualizada, id);
//
//         // Assert
//         _mockCondicionRepositorio.Verify(r => r.ObtenerPorId(recompensaId), Times.Once);
//         _mockCondicionRepositorio.Verify(r => r.Actualizar(It.Is<Condicion>(c => 
//             c.Recompensa != null && c.Recompensa.Id == recompensaId
//         )), Times.Once);
//     }
//
//     [Fact]
//     public async Task Actualizar_SinRecompensaId_DesasociaRecompensa()
//     {
//         // Arrange
//         const int id = 1;
//         var condicionExistente = new Condicion
//         {
//             Id = id,
//             Cantidad = 50,
//             NombreColumna = "EcoCoins",
//             RecompensaId = 2,
//             Recompensa = new Condicion { Id = 2 }
//         };
//         var condicionActualizada = new Condicion
//         {
//             Cantidad = 100,
//             NombreColumna = "EcoCoins",
//             RecompensaId = null
//         };
//
//         _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(id))
//             .ReturnsAsync(condicionExistente);
//         _mockCondicionRepositorio.Setup(r => r.Actualizar(It.IsAny<Condicion>()))
//             .Returns(Task.CompletedTask);
//
//         // Act
//         await _condicionLogica.Actualizar(condicionActualizada, id);
//
//         // Assert
//         _mockCondicionRepositorio.Verify(r => r.Actualizar(It.Is<Condicion>(c => 
//             c.Recompensa == null && c.RecompensaId == null
//         )), Times.Once);
//     }
//
//     [Fact]
//     public async Task Actualizar_RecompensaNoExiste_LanzaExcepcion()
//     {
//         // Arrange
//         const int id = 1;
//         const int recompensaId = 999;
//         var condicionExistente = new Condicion { Id = id };
//         var condicionActualizada = new Condicion
//         {
//             NombreColumna = "EcoCoins",
//             Cantidad = 100,
//             RecompensaId = recompensaId
//         };
//
//         _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(id))
//             .ReturnsAsync(condicionExistente);
//         _mockCondicionRepositorio.Setup(r => r.ObtenerPorId(recompensaId))
//             .ReturnsAsync((Condicion?)null);
//
//         // Act & Assert
//         await Assert.ThrowsAsync<CondicionExcepcion>(() => 
//             _condicionLogica.Actualizar(condicionActualizada, id));
//     }
//
//     [Fact]
//     public void FiltrarCondicionesSiCumplen_CondicionInvalidaSinColumnaNiEstructura_LanzaExcepcion()
//     {
//         // Arrange
//         var partida = new Partida
//         {
//             Recursos = new Recurso { EcoCoins = 100 }
//         };
//         var condicion = new Condicion
//         {
//             NombreColumna = null,
//             EstructuraId = null,
//             Cantidad = 100
//         };
//
//         // Act & Assert
//         Assert.Throws<CondicionExcepcion>(() => 
//             _condicionLogica.FiltrarCondicionSiCumple(condicion, partida));
//     }
//
//     [Fact]
//     public void FiltrarCondicionesSiCumplen_ColumnaInvalida_LanzaExcepcion()
//     {
//         // Arrange
//         var partida = new Partida
//         {
//             Recursos = new Recurso { EcoCoins = 100 }
//         };
//         var condicion = new Condicion
//         {
//             NombreColumna = "ColumnaInexistente",
//             Cantidad = 100
//         };
//
//         // Act & Assert
//         Assert.Throws<CondicionExcepcion>(() => 
//             _condicionLogica.FiltrarCondicionSiCumple(condicion, partida));
//     }
//
//     [Fact]
//     public void FiltrarCondicionesSiCumplen_CondicionPorEstructura_NoCumple()
//     {
//         // Arrange
//         var estructuraMapa = new List<EstructuraMapa>
//         {
//             new EstructuraMapa { EstructuraId = 1 }
//         };
//         var partida = new Partida
//         {
//             EstructuraMapa = estructuraMapa
//         };
//         var condicion = new Condicion
//         {
//             EstructuraId = 1,
//             Cantidad = 5 // Requiere 5, pero solo hay 1
//         };
//
//         // Act
//         var resultado = _condicionLogica.FiltrarCondicionSiCumple(condicion, partida);
//
//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Empty(resultado);
//     }
//
//     [Fact]
//     public void FiltrarCondicionesSiCumplen_MultiplesRecursos_RetornaSoloLasQueCumplen()
//     {
//         // Arrange
//         var partida = new Partida
//         {
//             Recursos = new Recurso
//             {
//                 EcoCoins = 200,
//                 Felicidad = 80,
//                 Energia = 30,
//                 Contaminacion = 0 // Valor por defecto
//             }
//         };
//         var condiciones = new List<Condicion>
//         {
//             new Condicion { NombreColumna = "EcoCoins", Cantidad = 150 }, // Cumple (200 >= 150)
//             new Condicion { NombreColumna = "Felicidad", Cantidad = 100 }, // No cumple (80 < 100)
//             new Condicion { NombreColumna = "Energia", Cantidad = 20 }, // Cumple (30 >= 20)
//             new Condicion { NombreColumna = "Contaminacion", Cantidad = 50 } // No cumple (0 < 50)
//         };
//
//         // Act
//         var resultado = _condicionLogica.FiltrarCondicionesSiCumplen(condiciones, partida);
//
//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(2, resultado.Count); // Solo EcoCoins y Energia cumplen
//     }
// }
//
//

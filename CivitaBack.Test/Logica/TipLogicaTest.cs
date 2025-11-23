// using CivitaBack.Domain.Entidades;
// using CivitaBack.Domain.Excepciones;
// using CivitaBack.Domain.Interfaces.Logica;
// using CivitaBack.Domain.Interfaces.Repositorios;
// using CivitaBack.Logica;
// using CivitaBack.Logica.Interfaces;
// using CivitaBack.Utils;
// using Moq;
//
// namespace CivitaBack.Tests;
//
// public class TipLogicaTest
// {
//     private readonly Mock<ITipsRepositorio> _mockTipsRepositorio;
//     private readonly Mock<ITipoTipRepositorio> _mockTipoTipRepositorio;
//     private readonly Mock<IUnidadDeTrabajo> _mockUow;
//     private readonly Mock<IAccesoUsuarios> _mockAccesoUsuarios;
//     private readonly ITipLogica _tipLogica;
//
//     public TipLogicaTest()
//     {
//         _mockTipsRepositorio = new Mock<ITipsRepositorio>();
//         _mockTipoTipRepositorio = new Mock<ITipoTipRepositorio>();
//         _mockUow = new Mock<IUnidadDeTrabajo>();
//         _mockAccesoUsuarios = new Mock<IAccesoUsuarios>();
//
//         _tipLogica = new TipLogica(
//             _mockTipsRepositorio.Object,
//             _mockTipoTipRepositorio.Object,
//             _mockUow.Object,
//             _mockAccesoUsuarios.Object
//         );
//
//         _mockUow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
//     }
//
//     [Fact]
//     public async Task ObtenerMsjPorIdTipo_IdValido_RetornaLista()
//     {
//         // Arrange
//         const int idTipo = 1;
//         var tipsMock = new List<Tip>
//         {
//             new Tip { Id = 1, Mensaje = "Tip 1", TipoId = idTipo },
//             new Tip { Id = 2, Mensaje = "Tip 2", TipoId = idTipo }
//         };
//
//         _mockTipsRepositorio.Setup(r => r.ObtenerMsjPorIdTipo(idTipo))
//             .ReturnsAsync(tipsMock);
//
//         // Act
//         var resultado = await _tipLogica.ObtenerMsjPorIdTipo(idTipo);
//
//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(2, resultado.Count);
//         _mockTipsRepositorio.Verify(r => r.ObtenerMsjPorIdTipo(idTipo), Times.Once);
//     }
//
//     [Fact]
//     public async Task Listado_HayTips_RetornaLista()
//     {
//         // Arrange
//         var tipsMock = new List<Tip>
//         {
//             new Tip { Id = 1, Mensaje = "Tip 1" },
//             new Tip { Id = 2, Mensaje = "Tip 2" }
//         };
//
//         _mockTipsRepositorio.Setup(r => r.ObtenerTodos())
//             .ReturnsAsync(tipsMock);
//
//         // Act
//         var resultado = await _tipLogica.Listado();
//
//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(2, resultado.Count);
//         _mockTipsRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
//     }
//
//     [Fact]
//     public async Task Crear_TipValido_CreaTip()
//     {
//         // Arrange
//         var tipoTip = new TipoTip { Id = 1, Descripcion = "Tipo Test" };
//         var tipNuevo = new Tip
//         {
//             TipoId = 1,
//             Mensaje = "Nuevo Tip",
//             Expresion = "expresion",
//             ElementoAdicional = "elemento",
//             EfectoFiltro = true
//         };
//
//         _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(1))
//             .ReturnsAsync(tipoTip);
//         _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
//         _mockTipsRepositorio.Setup(r => r.Agregar(It.IsAny<Tip>()))
//             .Returns(Task.CompletedTask);
//
//         // Act
//         await _tipLogica.Crear(tipNuevo);
//
//         // Assert
//         _mockTipoTipRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
//         _mockTipsRepositorio.Verify(r => r.Agregar(It.IsAny<Tip>()), Times.Once);
//         _mockUow.Verify(u => u.CommitAsync(), Times.Once);
//     }
//
//     [Fact]
//     public async Task Crear_TipoTipNoExiste_LanzaExcepcion()
//     {
//         // Arrange
//         var tipNuevo = new Tip { TipoId = 999 };
//
//         _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(999))
//             .ReturnsAsync((TipoTip?)null);
//
//         // Act & Assert
//         await Assert.ThrowsAsync<Exception>(() => _tipLogica.Crear(tipNuevo));
//         _mockTipsRepositorio.Verify(r => r.Agregar(It.IsAny<Tip>()), Times.Never);
//     }
//
//     [Fact]
//     public async Task Crear_NoEsAdmin_LanzaExcepcion()
//     {
//         // Arrange
//         var tipoTip = new TipoTip { Id = 1 };
//         var tipNuevo = new Tip { TipoId = 1 };
//
//         _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(1))
//             .ReturnsAsync(tipoTip);
//         _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);
//
//         // Act & Assert
//         await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipLogica.Crear(tipNuevo));
//         _mockTipsRepositorio.Verify(r => r.Agregar(It.IsAny<Tip>()), Times.Never);
//     }
//
//     [Fact]
//     public async Task Actualizar_TipValido_ActualizaTip()
//     {
//         // Arrange
//         const int idTip = 1;
//         var tipoTip = new TipoTip { Id = 1, Descripcion = "Tipo Test" };
//         var tipExistente = new Tip
//         {
//             Id = idTip,
//             TipoId = 1,
//             Mensaje = "Tip Original"
//         };
//         var tipActualizado = new Tip
//         {
//             TipoId = 1,
//             Mensaje = "Tip Actualizado",
//             Expresion = "nueva expresion",
//             ElementoAdicional = "nuevo elemento",
//             EfectoFiltro = false
//         };
//
//         _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(1))
//             .ReturnsAsync(tipoTip);
//         _mockTipsRepositorio.Setup(r => r.ObtenerPorId(idTip))
//             .ReturnsAsync(tipExistente);
//         _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(true);
//         _mockTipsRepositorio.Setup(r => r.Actualizar(It.IsAny<Tip>()))
//             .Returns(Task.CompletedTask);
//
//         // Act
//         await _tipLogica.Actualizar(tipActualizado, idTip);
//
//         // Assert
//         _mockTipoTipRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
//         _mockTipsRepositorio.Verify(r => r.ObtenerPorId(idTip), Times.Once);
//         _mockTipsRepositorio.Verify(r => r.Actualizar(It.IsAny<Tip>()), Times.Once);
//         _mockUow.Verify(u => u.CommitAsync(), Times.Once);
//     }
//
//     [Fact]
//     public async Task Actualizar_TipoTipNoExiste_LanzaExcepcion()
//     {
//         // Arrange
//         var tipActualizado = new Tip { TipoId = 999 };
//
//         _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(999))
//             .ReturnsAsync((TipoTip?)null);
//
//         // Act & Assert
//         await Assert.ThrowsAsync<Exception>(() => _tipLogica.Actualizar(tipActualizado, 1));
//     }
//
//     [Fact]
//     public async Task Actualizar_TipNoExiste_LanzaExcepcion()
//     {
//         // Arrange
//         var tipoTip = new TipoTip { Id = 1 };
//         var tipActualizado = new Tip { TipoId = 1 };
//
//         _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(1))
//             .ReturnsAsync(tipoTip);
//         _mockTipsRepositorio.Setup(r => r.ObtenerPorId(1))
//             .ReturnsAsync((Tip?)null);
//
//         // Act & Assert
//         await Assert.ThrowsAsync<Exception>(() => _tipLogica.Actualizar(tipActualizado, 1));
//     }
//
//     [Fact]
//     public async Task Actualizar_NoEsAdmin_LanzaExcepcion()
//     {
//         // Arrange
//         var tipoTip = new TipoTip { Id = 1 };
//         var tipExistente = new Tip { Id = 1, TipoId = 1 };
//         var tipActualizado = new Tip { TipoId = 1 };
//
//         _mockTipoTipRepositorio.Setup(r => r.ObtenerPorId(1))
//             .ReturnsAsync(tipoTip);
//         _mockTipsRepositorio.Setup(r => r.ObtenerPorId(1))
//             .ReturnsAsync(tipExistente);
//         _mockAccesoUsuarios.Setup(a => a.EsDios()).Returns(false);
//
//         // Act & Assert
//         await Assert.ThrowsAsync<AccesoDenegadoExcepcion>(() => _tipLogica.Actualizar(tipActualizado, 1));
//     }
//
//     [Fact]
//     public async Task ObtenerPorIdTipo_IdValido_RetornaTip()
//     {
//         // Arrange
//         const int idTip = 1;
//         var tipMock = new Tip
//         {
//             Id = idTip,
//             Mensaje = "Test Tip"
//         };
//
//         _mockTipsRepositorio.Setup(r => r.ObtenerPorId(idTip))
//             .ReturnsAsync(tipMock);
//
//         // Act
//         var resultado = await _tipLogica.ObtenerPorIdTipo(idTip);
//
//         // Assert
//         Assert.NotNull(resultado);
//         Assert.Equal(idTip, resultado.Id);
//         _mockTipsRepositorio.Verify(r => r.ObtenerPorId(idTip), Times.Once);
//     }
//
//     [Fact]
//     public async Task ObtenerPorIdTipo_TipNoExiste_RetornaNull()
//     {
//         // Arrange
//         const int idTip = 999;
//
//         _mockTipsRepositorio.Setup(r => r.ObtenerPorId(idTip))
//             .ReturnsAsync((Tip?)null);
//
//         // Act
//         var resultado = await _tipLogica.ObtenerPorIdTipo(idTip);
//
//         // Assert
//         Assert.Null(resultado);
//     }
// }
//

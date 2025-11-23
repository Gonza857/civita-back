using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica;
using Moq;

namespace CivitaBack.Tests;

public class UsuarioLogicaTest
{
    private readonly Mock<IUsuarioRepositorio> _mockUsuarioRepositorio;
    private readonly IUsuarioLogica _usuarioLogica;

    public UsuarioLogicaTest()
    {
        _mockUsuarioRepositorio = new Mock<IUsuarioRepositorio>();
        _usuarioLogica = new UsuarioLogica(_mockUsuarioRepositorio.Object);
    }

    [Fact]
    public async Task ObtenerPorId_UsuarioExiste_RetornaUsuario()
    {
        // Arrange
        const int idUsuario = 1;
        var usuarioMock = new Usuario
        {
            Id = idUsuario,
            NombreUsuario = "testuser",
            Mail = "test@test.com"
        };

        _mockUsuarioRepositorio.Setup(r => r.ObtenerPorId(idUsuario))
            .ReturnsAsync(usuarioMock);

        // Act
        var resultado = await _usuarioLogica.ObtenerPorId(idUsuario);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(idUsuario, resultado.Id);
        _mockUsuarioRepositorio.Verify(r => r.ObtenerPorId(idUsuario), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorId_UsuarioNoExiste_RetornaNull()
    {
        // Arrange
        const int idUsuario = 999;

        _mockUsuarioRepositorio.Setup(r => r.ObtenerPorId(idUsuario))
            .ReturnsAsync((Usuario?)null);

        // Act && Assert
        await Assert.ThrowsAsync<DominioException>(() => _usuarioLogica.ObtenerPorId(idUsuario));
        _mockUsuarioRepositorio.Verify(r => r.ObtenerPorId(idUsuario), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorCorreo_UsuarioExiste_RetornaUsuario()
    {
        // Arrange
        const string correo = "test@test.com";
        var usuarioMock = new Usuario
        {
            Id = 1,
            NombreUsuario = "testuser",
            Mail = correo
        };

        _mockUsuarioRepositorio.Setup(r => r.ObtenerUsuarioPorMail(correo))
            .ReturnsAsync(usuarioMock);

        // Act
        var resultado = await _usuarioLogica.ObtenerPorCorreo(correo);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(correo, resultado.Mail);
        _mockUsuarioRepositorio.Verify(r => r.ObtenerUsuarioPorMail(correo), Times.Once);
    }

    [Fact]
    public async Task ObtenerPorCorreo_UsuarioNoExiste_LanzaExcepcion()
    {
        // Arrange
        const string correo = "noexiste@test.com";

        _mockUsuarioRepositorio.Setup(r => r.ObtenerUsuarioPorMail(correo))
            .ReturnsAsync((Usuario?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<UsuarioExcepcion>(() => _usuarioLogica.ObtenerPorCorreo(correo));
        Assert.Contains("No se encontró el usuario", ex.Message);
        _mockUsuarioRepositorio.Verify(r => r.ObtenerUsuarioPorMail(correo), Times.Once);
    }

    [Fact]
    public async Task ObtenerUsuarioPorNombre_UsuarioExiste_RetornaUsuario()
    {
        // Arrange
        const string nombre = "testuser";
        var usuarioMock = new Usuario
        {
            Id = 1,
            NombreUsuario = nombre,
            Mail = "test@test.com"
        };

        _mockUsuarioRepositorio.Setup(r => r.ObtenerUsuarioPorNombre(nombre))
            .ReturnsAsync(usuarioMock);

        // Act
        var resultado = await _usuarioLogica.ObtenerUsuarioPorNombre(nombre);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(nombre, resultado.NombreUsuario);
        _mockUsuarioRepositorio.Verify(r => r.ObtenerUsuarioPorNombre(nombre), Times.Once);
    }

    [Fact]
    public async Task ObtenerUsuarioPorNombre_UsuarioNoExiste_RetornaNull()
    {
        // Arrange
        const string nombre = "noexiste";

        _mockUsuarioRepositorio.Setup(r => r.ObtenerUsuarioPorNombre(nombre))
            .ReturnsAsync((Usuario?)null);

        // Act
        var resultado = await _usuarioLogica.ObtenerUsuarioPorNombre(nombre);

        // Assert
        Assert.Null(resultado);
        _mockUsuarioRepositorio.Verify(r => r.ObtenerUsuarioPorNombre(nombre), Times.Once);
    }
}


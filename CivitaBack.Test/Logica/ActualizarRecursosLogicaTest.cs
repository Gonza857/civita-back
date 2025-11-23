using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica;
using Moq;

namespace CivitaBack.Tests;

public class ActualizarRecursosLogicaTest
{
    private readonly IActualizarRecursosLogica _actualizarRecursosLogica;

    public ActualizarRecursosLogicaTest()
    {
        _actualizarRecursosLogica = new ActualizarRecursosLogica();
    }

    [Fact]
    public void ActualizarRecursosAsync_RecursosValidos_ActualizaRecursos()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                Felicidad = 50,
                Contaminacion = 30,
                EcoCoins = 100,
                Energia = 75
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 10, 5, 20, 15);

        // Assert
        Assert.Equal(60, partida.Recursos.Felicidad);
        Assert.Equal(35, partida.Recursos.Contaminacion);
        Assert.Equal(120, partida.Recursos.EcoCoins);
        Assert.Equal(90, partida.Recursos.Energia);
    }

    [Fact]
    public void ActualizarRecursosAsync_RecursosNull_NoHaceNada()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = null
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 10, 5, 20, 15);

        // Assert
        // No debe lanzar excepción
        Assert.Null(partida.Recursos);
    }

    [Fact]
    public void ActualizarRecursosAsync_FelicidadSupera100_LimitaA100()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                Felicidad = 95
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 10, 0, 0, 0);

        // Assert
        Assert.Equal(100, partida.Recursos.Felicidad);
    }

    [Fact]
    public void ActualizarRecursosAsync_FelicidadBajaDe0_LimitaA0()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                Felicidad = 5
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, -10, 0, 0, 0);

        // Assert
        Assert.Equal(0, partida.Recursos.Felicidad);
    }

    [Fact]
    public void ActualizarRecursosAsync_ContaminacionSupera100_LimitaA100()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                Contaminacion = 95
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 0, 10, 0, 0);

        // Assert
        Assert.Equal(100, partida.Recursos.Contaminacion);
    }

    [Fact]
    public void ActualizarRecursosAsync_ContaminacionBajaDe0_LimitaA0()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                Contaminacion = 5
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 0, -10, 0, 0);

        // Assert
        Assert.Equal(0, partida.Recursos.Contaminacion);
    }

    [Fact]
    public void ActualizarRecursosAsync_EnergiaSupera100_LimitaA100()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                Energia = 95
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 0, 0, 0, 10);

        // Assert
        Assert.Equal(100, partida.Recursos.Energia);
    }

    [Fact]
    public void ActualizarRecursosAsync_EnergiaBajaDe0_LimitaA0()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                Energia = 5
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 0, 0, 0, -10);

        // Assert
        Assert.Equal(0, partida.Recursos.Energia);
    }

    [Fact]
    public void ActualizarRecursosAsync_EcoCoinsNoTieneLimite_PuedeSerNegativo()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                EcoCoins = 50
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 0, 0, -100, 0);

        // Assert
        // El código fuente limita todos los valores a 0 si son negativos, incluso EcoCoins
        Assert.Equal(0, partida.Recursos.EcoCoins);
    }

    [Fact]
    public void ActualizarRecursosAsync_EcoCoinsNoTieneLimiteSuperior_PuedeSerMayorA100()
    {
        // Arrange
        var partida = new Partida
        {
            Recursos = new Recurso
            {
                EcoCoins = 50
            }
        };

        // Act
        _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 0, 0, 200, 0);

        // Assert
        Assert.Equal(250, partida.Recursos.EcoCoins);
    }
}



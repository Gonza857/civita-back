using CivitaBack.Domain.Entidades;

namespace CivitaBack.Tests;

public static class TestData
{
    public static Usuario CrearUsuarioBase() => new Usuario
    {
        Id = 1,
        NombreUsuario = "Jugador1",
        Mail = "test@ejemplo.com"
    };

    public static Partida CrearPartida(Usuario usuario) => new Partida
    {
        Id = 1,
        Usuario = usuario,
        LogroPartidas = new List<LogroPartida>()
    };

    public static void LlenarPartidaConLogros(Partida partida, List<Logro> logrosCompletados)
    {
        foreach (Logro logro in logrosCompletados)
        {
            LogroPartida lp = new LogroPartida
            {
                PartidaId = partida.Id,
                LogroId = logro.Id,
                Logro = logro
            };
            partida.LogroPartidas.Add(lp);
        }
    }

    public static Recurso CrearRecurso(
        Partida partida,
        int energiaQty, int felicidadQty, int contaminacionQty, int dineroQty
        ) => new Recurso
        {
            Id = 1,
            Energia = energiaQty,
            Felicidad = felicidadQty,
            EcoCoins = dineroQty,
            Contaminacion = contaminacionQty,
            Partida = partida
        };
    public static TipoLogro CrearTipoLogro(string tipo) => new TipoLogro
    {
        Id = 1,
        Nombre = tipo
    };
    public static Logro CrearLogro(int id, TipoLogro tipo, Condicion condicion, string titulo) => new Logro
    {
        Id = id,
        Titulo = titulo,
        Descripcion = "Descripción test",
        TipoLogro = tipo,
        Condicion = condicion,
    };

    public static Condicion CrearCondicion(string nombreColumna, int cantidad) => new Condicion
    {
        Id = 1,
        Cantidad = cantidad,
        NombreColumna = nombreColumna,
        EstructuraId = 0,
    };

    public static Recompensa CrearRecompensa(int cantidad, string? columna = null, int? estructuraId = null) => new Recompensa
    {
        Id = 1,
        NombreColumna = columna,
        Cantidad = cantidad,
        EstructuraId = estructuraId,
    };

    public static Condicion CrearRecompensaDTO(int cantidad, string? columna = null, int? estructuraId = null) => new Condicion
    {
        Id = 1,
        NombreColumna = columna,
        Cantidad = cantidad,
        EstructuraId = estructuraId,
    };

}
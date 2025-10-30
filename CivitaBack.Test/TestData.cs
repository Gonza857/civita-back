using CivitaBack.Data.BO;

namespace CivitaBack.Tests;

public static class TestData
{
    public static Usuario CrearUsuarioBase() => new Usuario
    {
        Id = 1,
        NombreUsuario = "Jugador1",
        Mail = "test@ejemplo.com"
    };
    
    public static PartidaEF CrearPartida(Usuario usuario) => new PartidaEF
    {
        Id = 1,
        Usuario = usuario,
        LogroPartidas = new List<LogroPartidaEF>()
    };

    public static void LlenarPartidaConLogros(PartidaEF partida, List<LogroEF> logrosCompletados)
    {
        foreach (LogroEF logro in logrosCompletados)
        {
            LogroPartidaEF lp = new LogroPartidaEF
            {
                PartidaId = partida.Id,
                LogroId = logro.Id,
                Logro = logro
            };
            partida.LogroPartidas.Add(lp);
        }
    }
    
    public static Recurso CrearRecurso(
        PartidaEF partida, 
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
    public static LogroEF CrearLogro(int id, TipoLogro tipo, CondicionEF condicion, string titulo) => new LogroEF
    {
        Id = id,
        Titulo = titulo,
        Descripcion = "Descripción test",
        TipoLogro = tipo,
        Condicion = condicion,
    };
    
    public static CondicionEF CrearCondicion(string nombreColumna, int cantidad) => new CondicionEF
    {
        Id = 1,
        Cantidad = cantidad,
        NombreColumna = nombreColumna,
        EstructuraId = 0,
    };

    public static CondicionEF CrearRecompensa(int cantidad, string? columna = null, int? estructuraId = null) => new CondicionEF
    {
        Id = 1,
        NombreColumna = columna,
        Cantidad = cantidad,
        EstructuraId = estructuraId,
        EsRecompensa = true,
    };
    
    public static CondicionDTO CrearRecompensaDTO(int cantidad, string? columna = null, int? estructuraId = null) => new CondicionDTO
    {
        Id = 1,
        NombreColumna = columna,
        Cantidad = cantidad,
        EstructuraId = estructuraId,
        EsRecompensa = true,
    };

}
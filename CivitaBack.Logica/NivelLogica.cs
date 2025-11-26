using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface INivelLogica
{
    void SubirNivel(Partida partida);
    int ObtenerExperienciaFaltanteParaSiguienteNivel(int nivel, int experiencia);
    
    bool PuedeSubir (int xpActual, int nivelActual);

    Task VerificarNivel(Partida partida);

    int ObtenerExperienciaTechoNivel(int nivel);
}

public class NivelLogica : INivelLogica
{
    private readonly int BASE_XP = 100; // xp para subir a nivel 1
    private readonly int INCREMENTO_XP = 150; // incremento entre niveles
    private readonly IPartidaRepositorio _partidaRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    
    public NivelLogica(IPartidaRepositorio partidaRepositorio, IUnidadDeTrabajo unidadDeTrabajo)
    {
        this._partidaRepositorio = partidaRepositorio;
        this._unidadDeTrabajo = unidadDeTrabajo;
    }
    
    private int XpNecesariaParaSiguienteNivel(int nivelActual)
    {
        // Fórmula correcta:
        return BASE_XP + INCREMENTO_XP * nivelActual;
    }

    public bool PuedeSubir(int xpActual, int nivelActual)
    {
        int xpNecesaria = XpNecesariaParaSiguienteNivel(nivelActual);
        return xpActual >= xpNecesaria;
    }

    public async Task VerificarNivel(Partida partida)
    {
        // int nivelPrevio = partida.Nivel;
        this.SubirNivel(partida);
        await this._partidaRepositorio.Actualizar(partida);
        await this._unidadDeTrabajo.CommitAsync();
    }

    /// <summary>
    /// Sube el nivel de la partida, consumiendo el exceso de experiencia.
    /// Esto maneja múltiples subidas de nivel en una sola llamada.
    /// </summary>
    public void SubirNivel(Partida partida)
    {
        while (true)
        {
            // Calcular el UMBRAL TOTAL de XP requerido para alcanzar el nivel actual + 1.
            int umbralProximoNivel = CalcularCostoTotalAcumulado(partida.Nivel + 1);

            if (partida.Experiencia >= umbralProximoNivel)
            {
                partida.Nivel++; // Sube el nivel
            }
            else
            {
                break; // XP acumulado es menor que el umbral del siguiente nivel
            }
        }
    }
    
    /// <summary>
    /// Calcula la experiencia total acumulada que se necesita para alcanzar un nivel objetivo.
    /// </summary>
    /// <param name="nivelObjetivo">El nivel que se intenta alcanzar (ej: si el nivel actual es 1, el objetivo es 2).</param>
    /// <returns>La cantidad total de XP requerida.</returns>
    private int CalcularCostoTotalAcumulado(int nivelObjetivo)
    {
        if (nivelObjetivo <= 1) return 0; // Se asume que el nivel 1 requiere 0 XP total

        int xpTotal = 0;
        // Suma el costo incremental para cada nivel de 1 hasta el objetivo
        for (int n = 1; n < nivelObjetivo; n++)
        {
            // El costo incremental para pasar de N a N+1 es: BASE_XP + INCREMENTO_XP * (N-1)
            xpTotal += BASE_XP + (INCREMENTO_XP * (n - 1));
        }
        return xpTotal;
    }

    public int ObtenerExperienciaFaltanteParaSiguienteNivel(int nivel, int experiencia)
    {
        int xpUmbralTotalSiguiente = CalcularCostoTotalAcumulado(nivel + 1); 
        int xpFaltante = xpUmbralTotalSiguiente - experiencia; 
        return xpFaltante;
    }

    public int ObtenerExperienciaTechoNivel(int nivel)
    {
        return CalcularCostoTotalAcumulado(nivel + 1);
    }
    
}
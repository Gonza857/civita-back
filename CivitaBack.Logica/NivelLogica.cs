using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

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
        // Fórmula: Costo Incremental para pasar de N a N+1
        return BASE_XP + INCREMENTO_XP * nivelActual;
    }

    /// <inheritdoc />
    public bool PuedeSubir(int xpActual, int nivelActual)
    {
        // Usa el cálculo del umbral total para verificar si puede subir (aunque SubirNivel ya hace esto).
        int xpUmbralTotalSiguiente = CalcularCostoTotalAcumulado(nivelActual + 1);
        return xpActual >= xpUmbralTotalSiguiente;
    }

    /// <inheritdoc />
    public async Task VerificarNivel(Partida partida)
    {
        int nivelPrevio = partida.Nivel;
        
        this.SubirNivel(partida);
        
        if (nivelPrevio != partida.Nivel)
        {
            // Si el nivel cambió, persistir los cambios de Nivel/XP
            await this._partidaRepositorio.Actualizar(partida);
            await this._unidadDeTrabajo.CommitAsync();
        }
        await this._partidaRepositorio.Actualizar(partida);
        await this._unidadDeTrabajo.CommitAsync();

    }

    /// <inheritdoc />
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
    /// Este es el umbral.
    /// </summary>
    private int CalcularCostoTotalAcumulado(int nivelObjetivo)
    {
        if (nivelObjetivo <= 1) return 0;

        int xpTotal = 0;
        // Suma el costo incremental para cada nivel de 1 hasta el objetivo (nivelObjetivo - 1)
        for (int n = 1; n < nivelObjetivo; n++)
        {
            // El costo incremental para pasar de N a N+1 es: BASE_XP + INCREMENTO_XP * (N-1)
            xpTotal += BASE_XP + (INCREMENTO_XP * (n - 1));
        }
        return xpTotal;
    }

    /// <inheritdoc />
    public int ObtenerExperienciaFaltanteParaSiguienteNivel(int nivelActual, int experienciaActual)
    {
        // XP Total necesaria para el siguiente nivel
        int xpUmbralTotalSiguiente = CalcularCostoTotalAcumulado(nivelActual + 1); 
        
        // XP que falta = Umbral Siguiente - XP Actual
        int xpFaltante = xpUmbralTotalSiguiente - experienciaActual; 
        
        return xpFaltante;
    }

    /// <inheritdoc />
    public int ObtenerExperienciaTechoNivel(int nivel)
    {
        // El "techo" de un nivel N es el umbral total que requiere el nivel N+1.
        return CalcularCostoTotalAcumulado(nivel + 1);
    }
    
}
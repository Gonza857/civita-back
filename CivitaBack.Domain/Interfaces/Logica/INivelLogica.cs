using CivitaBack.Domain.Entidades;

namespace CivitaBack.Domain.Interfaces.Logica;

public interface INivelLogica
{
    /// <summary>
    /// Intenta subir el nivel de la partida basándose en la experiencia acumulada actual.
    /// Este método modifica la entidad Partida en memoria.
    /// </summary>
    /// <param name="partida">La entidad Partida con las propiedades Nivel y Experiencia.</param>
    void SubirNivel(Partida partida);

    /// <summary>
    /// Calcula la cantidad de experiencia que le falta al jugador para alcanzar el siguiente nivel.
    /// </summary>
    /// <param name="nivel">El nivel actual del jugador.</param>
    /// <param name="experiencia">La experiencia acumulada total actual del jugador.</param>
    /// <returns>La cantidad de XP que falta (un número positivo) o que tiene de sobra (número negativo o cero).</returns>
    int ObtenerExperienciaFaltanteParaSiguienteNivel(int nivel, int experiencia);
    
    /// <summary>
    /// Determina si la experiencia acumulada actual es suficiente para alcanzar el siguiente nivel.
    /// </summary>
    /// <param name="xpActual">La experiencia total acumulada.</param>
    /// <param name="nivelActual">El nivel actual del jugador.</param>
    /// <returns>True si tiene suficiente XP para subir al siguiente nivel.</returns>
    bool PuedeSubir(int xpActual, int nivelActual);

    /// <summary>
    /// Verifica si la Partida ha subido de nivel. Si es así, actualiza la base de datos.
    /// </summary>
    /// <param name="partida">La entidad Partida con la XP ya actualizada.</param>
    Task VerificarNivel(Partida partida);

    /// <summary>
    /// Obtiene el umbral total de experiencia requerido para subir del nivel actual al siguiente.
    /// </summary>
    /// <param name="nivel">El nivel actual del jugador.</param>
    /// <returns>La experiencia total acumulada necesaria para alcanzar el nivel siguiente.</returns>
    int ObtenerExperienciaTechoNivel(int nivel);
}
using CivitaBack.Domain.Entidades;

namespace CivitaBack.Logica;

public interface INivelLogica
{
    void SubirNivel(ref int nivel, int experiencia);
    int ObtenerExperienciaFaltanteParaSiguienteNivel(int nivel, int experiencia);
    
    bool PuedeSubir (int xpActual, int nivelActual);

}

public class NivelLogica : INivelLogica
{
    private readonly int BASE_XP = 100; // xp para subir a nivel 1
    private readonly int INCREMENTO_XP = 150; // incremento entre niveles

    public NivelLogica()
    {
        
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

    public void SubirNivel(ref int nivel, int experiencia)
    {
        if (!PuedeSubir(experiencia, nivel))
            throw new Exception("No puede subir de nivel todavía");
        nivel++;
    }

    public int ObtenerExperienciaFaltanteParaSiguienteNivel(int nivel, int experiencia)
    {
        Console.WriteLine("Nivel actual: " + nivel);
        int xpNecesaria = XpNecesariaParaSiguienteNivel(nivel);
        return xpNecesaria - experiencia;
    }
    
}
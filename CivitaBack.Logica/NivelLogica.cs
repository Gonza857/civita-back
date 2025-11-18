using CivitaBack.Domain.Entidades;

namespace CivitaBack.Logica;

public interface INivelLogica
{
    void SubirNivel(int nivel, int experiencia);
    int ObtenerExperienciaFaltanteParaSiguienteNivel(int nivel, int experiencia);

}

public class NivelLogica : INivelLogica
{
    private readonly int BASE_XP = 100; // xp para subir a nivel 1
    private readonly int INCREMENTO_XP = 150; // incremento entre niveles

    public NivelLogica()
    {
        
    }
    
    private int XPNecesariaParaSiguienteNivel(int nivelActual)
    {
        // Fórmula correcta:
        return BASE_XP + INCREMENTO_XP * nivelActual;
    }

    private bool PuedeSubir(int xpActual, int nivelActual)
    {
        int xpNecesaria = XPNecesariaParaSiguienteNivel(nivelActual);
        return xpActual >= xpNecesaria;
    }

    public void SubirNivel(int nivel, int experiencia)
    {
        if (!PuedeSubir(experiencia, nivel))
            throw new Exception("No puede subir de nivel todavía");

        Console.WriteLine("Subió de nivel!");
    }

    public int ObtenerExperienciaFaltanteParaSiguienteNivel(int nivel, int experiencia)
    {
        int xpNecesaria = XPNecesariaParaSiguienteNivel(nivel);
        return xpNecesaria - experiencia;
    }
    
}
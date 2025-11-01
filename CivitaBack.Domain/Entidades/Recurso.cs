using CivitaBack.Domain.Common;

namespace CivitaBack.Domain.Entidades;

public class Recurso : Auditable
{
    public int Id { get; set; }

    public int Energia { get; set; }
    public int Contaminacion { get; set; }
    public int Felicidad { get; set; }
    public int EcoCoins { get; set; }
    public int Poblacion { get; set; }

    public int PartidaId { get; set; }
    public Partida? Partida { get; set; }

    public Recurso() { }

    public Recurso(int energia, int contaminacion, int felicidad, int ecoCoins, Partida partida)
    {
        Energia = energia;
        Contaminacion = contaminacion;
        Felicidad = felicidad;
        EcoCoins = ecoCoins;
        PartidaId = partida.Id;
        Partida = partida;
    }
}

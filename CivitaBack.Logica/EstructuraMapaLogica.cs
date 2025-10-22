using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;

namespace CivitaBack.Logica;

public interface IEstructuraMapaLogica
{
    void Colocar (Estructura e, Partida p);

    Task ReiniciarEstructurasDePartida(int idPartida);

}
public class EstructuraMapaLogica : IEstructuraMapaLogica
{
    private readonly IEstructuraMapaRepositorio estructuraMapaRepositorio;

    public EstructuraMapaLogica(IEstructuraMapaRepositorio emr)
    {
        estructuraMapaRepositorio = emr;
    }

    public void Colocar(Estructura e, Partida p)
    {
        EstructuraMapa em = new EstructuraMapa
        {
           Estructura = e,
           Partida = p
        };
        this.estructuraMapaRepositorio.GuardarCambios();
    }

    public async Task ReiniciarEstructurasDePartida(int idPartida)
    {
        await this.estructuraMapaRepositorio.EliminarPorPartidaIdAsync(idPartida);
    }
}

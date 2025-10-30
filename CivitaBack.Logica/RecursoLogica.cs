using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Enum;
using CivitaBack.Data.Repositorio;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface IRecursoLogica
{
    Task ConfigurarInicial(PartidaEF partida);

    Task<RecursoDTO> ObtenerRecursos(int idPartida);
    Task ModificarEnergia(int idPartida, int cantidad);


}

public class RecursoLogica : IRecursoLogica, IParser<Recurso, RecursoDTO>
{

    private readonly IRecursoRepositorio repositorioRecurso;

    public RecursoLogica(IRecursoRepositorio rr)
    {
        repositorioRecurso = rr;
    }

    public async Task ConfigurarInicial(PartidaEF partida)
    {
        if (partida == null) throw new PartidaExcepcion("No se proporcionó Partida");
        Recurso recurso = new Recurso
        {
            PartidaId = partida.Id,
            EcoCoins = 450,
            Felicidad = 40,
            Energia = 30,
            Contaminacion = 60,
        };
        await this.repositorioRecurso.GuardarRecurso(recurso);
    }

    public async Task<RecursoDTO> ObtenerRecursos(int idPartida)
    {
        Recurso recursoPartida = await this.repositorioRecurso.ObtenerRecursosPartida(idPartida);

        // Si no hay recursos o son menos de 4, error
        if (recursoPartida == null)
            throw new PartidaExcepcion("No se encontraron recursos para la partida especificada.");
        
        return new RecursoDTO
        {
            Energia = recursoPartida.Energia,
            Felicidad = recursoPartida.Felicidad,
            EcoCoins = recursoPartida.EcoCoins,
            Contaminacion = recursoPartida.Contaminacion,
        };
    }

    public RecursoDTO ToDto(Recurso entidad)
    {
        return new();
    }


public async Task ModificarEnergia(int idPartida, int cantidad)
    {
        // Buscamos el recurso "Energía" de la partida
        Recurso recurso = await repositorioRecurso.ObtenerRecursosPartida(idPartida);

        if (recurso == null)
            throw new PartidaExcepcion("No se encontró el recurso Energía para la partida.");

        // Ajustamos el valor
        recurso.Energia += cantidad;

        // Controlamos los límites (0 - 100)
        if (recurso.Energia > 100) recurso.Energia = 100;
        if (recurso.Energia < 0) recurso.Energia = 0;

        // Guardamos los cambios
        await repositorioRecurso.Actualizar(recurso);
    }
}

using CivitaBack.Data.DTO;
using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Excepciones;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface IRecursoLogica
{
    Task ConfigurarInicial(Partida partida);

    Task<RecursoDTO> ObtenerRecursos(int idPartida);
    Task ModificarEnergia(int idPartida, int cantidad);
    
}

public class RecursoLogica : IRecursoLogica, IParser<Recurso, RecursoDTO>
{

    private readonly IRecursoRepositorio _repositorioRecurso;

    public RecursoLogica(IRecursoRepositorio rr)
    {
        _repositorioRecurso = rr;
    }

    public async Task ConfigurarInicial(Partida partida)
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
        await this._repositorioRecurso.Agregar(recurso);
    }

    public async Task<RecursoDTO> ObtenerRecursos(int idPartida)
    {
        Recurso recursoPartida = await this._repositorioRecurso.ObtenerRecursosPartida(idPartida);

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
        Recurso recurso = await _repositorioRecurso.ObtenerRecursosPartida(idPartida);

        if (recurso == null)
            throw new PartidaExcepcion("No se encontró el recurso Energía para la partida.");

        // Ajustamos el valor
        recurso.Energia += cantidad;

        // Controlamos los límites (0 - 100)
        if (recurso.Energia > 100) recurso.Energia = 100;
        if (recurso.Energia < 0) recurso.Energia = 0;

        // Guardamos los cambios
        await _repositorioRecurso.Actualizar(recurso);
    }
}

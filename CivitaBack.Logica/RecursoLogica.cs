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
    void ConfigurarInicial(Partida partida);

    RecursoDTO ObtenerRecursos(int idPartida);
    void ModificarEnergia(int idPartida, int cantidad);


}

public class RecursoLogica : IRecursoLogica, IParser<Recurso, RecursoDTO>
{

    private readonly IRecursoRepositorio repositorioRecurso;

    public RecursoLogica(IRecursoRepositorio rr)
    {
        repositorioRecurso = rr;
    }

    public void ConfigurarInicial(Partida partida)
    {
        List<Recurso> recursos = new List<Recurso>
        {
            new Recurso("Energia", 100, partida),
            new Recurso("Felicidad", 50, partida),
            new Recurso("EcoCoins", 200, partida),
            new Recurso("Contaminación", 60, partida)
        };
        this.repositorioRecurso.GuardarVarios(recursos);
    }

    public RecursoDTO ObtenerRecursos(int idPartida)
    {
        List<Recurso> recursosDePartida = this.repositorioRecurso.ObtenerRecursosPartida(idPartida);

        // Si no hay recursos o son menos de 4, error
        if (recursosDePartida == null || recursosDePartida.Count < 4)
            throw new PartidaExcepcion("No se encontraron recursos para la partida especificada.");


        return new RecursoDTO
        {
            Energia = recursosDePartida.First(r => r.Nombre == TipoRecurso.Energia.GetDescription()).Cantidad,
            Felicidad = recursosDePartida.First(r => r.Nombre == TipoRecurso.Felicidad.GetDescription()).Cantidad,
            EcoCoins = recursosDePartida.First(r => r.Nombre == TipoRecurso.EcoCoins.GetDescription()).Cantidad,
            Contaminacion = recursosDePartida.First(r => r.Nombre == TipoRecurso.Contaminacion.GetDescription()).Cantidad,
        };
    }

    public RecursoDTO ToDto(Recurso entidad)
    {
        return new();
    }


public void ModificarEnergia(int idPartida, int cantidad)
    {
        // Buscamos el recurso "Energía" de la partida
        var recurso = repositorioRecurso.ObtenerRecursosPartida(idPartida)
            .FirstOrDefault(r => r.Nombre == TipoRecurso.Energia.GetDescription());

        if (recurso == null)
            throw new PartidaExcepcion("No se encontró el recurso Energía para la partida.");

        // Ajustamos el valor
        recurso.Cantidad += cantidad;

        // Controlamos los límites (0 - 100)
        if (recurso.Cantidad > 100) recurso.Cantidad = 100;
        if (recurso.Cantidad < 0) recurso.Cantidad = 0;

        // Guardamos los cambios
        repositorioRecurso.Actualizar(recurso);
    }
}

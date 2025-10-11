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
            new Recurso("Energía", 0, partida),
            new Recurso("Felicidad", 0, partida),
            new Recurso("EcoCoins", 0, partida),
            new Recurso("Contaminación", 0, partida)
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
}

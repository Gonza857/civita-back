using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Data.Repositorio;
using CivitaBack.Utils;

namespace CivitaBack.Logica;

public interface IRecursoLogica
{
    void ConfigurarInicial(Partida partida);

    List<RecursoDTO> ObtenerRecursos(int idPartida);
    
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
            new Recurso("Contaminacion", 0, partida)
        };
        this.repositorioRecurso.GuardarVarios(recursos);
    }

    public List<RecursoDTO> ObtenerRecursos(int idPartida)
    {
        List<Recurso> recursosDePartida = this.repositorioRecurso.ObtenerRecursosPartida(idPartida);
        return recursosDePartida
            .Select(p => this.ToDto(p))
            .ToList();
    }

    public RecursoDTO ToDto(Recurso entidad)
    {
        return new RecursoDTO
        {
            Cantidad = entidad.Cantidad,
            Id = entidad.Id,
            Nombre = entidad.Nombre,
        };
    }
}

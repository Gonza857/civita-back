using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Utils;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Data.DTO;

namespace CivitaBack.Logica;

public class RecursoLogica : IRecursoLogica
{
    private readonly IRecursoRepositorio _repositorioRecurso;
    private readonly IUnidadDeTrabajo _uow;
    private readonly IAccesoUsuarios _accesoUsuarios;
    private readonly IActualizarRecursosLogica _crudRecursosLogica;
    private readonly IPartidaRepositorio _partidaRepositorio; 

    public RecursoLogica(IRecursoRepositorio rr, IUnidadDeTrabajo uow, IAccesoUsuarios accesoUsuarios, IActualizarRecursosLogica crl,IPartidaRepositorio pr)
    {
        _repositorioRecurso = rr;
        _uow = uow;
        _accesoUsuarios = accesoUsuarios;
        _crudRecursosLogica = crl;
        _partidaRepositorio = pr; 
    }

    public async Task ConfigurarInicial(Partida partida)
    {
        if (partida == null) throw new PartidaExcepcion("No se proporcionó Partida");
        Recurso r = await this._repositorioRecurso.ObtenerPorId(partida.Id);
        if (r is null) throw new Exception("No se encontraron los recursos");
        r.EcoCoins = 450;
        r.Felicidad = 40;
        r.Energia = 30;
        r.Contaminacion = 60;
        await this._repositorioRecurso.Actualizar(r);
        await this._uow.CommitAsync();
    }

    public async Task<Recurso> ObtenerRecursos(int idPartida)
    {
        Recurso recursoPartida = await this._repositorioRecurso.ObtenerRecursosPartida(idPartida);

        // Si no hay recursos o son menos de 4, error
        if (recursoPartida == null)
            throw new PartidaExcepcion("No se encontraron recursos para la partida especificada.");

        //_accesoUsuarios.ValidarAcceso(recursoPartida.Partida.UsuarioId);

        return recursoPartida;
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
        await this._uow.CommitAsync();
    }

    public async Task<Recurso> ImpactarPremiosMiniJuego(Partida partida, Recurso recursos)
    {
        Recurso recursoAdevolver = new Recurso(); 
        
        _crudRecursosLogica.ActualizarRecursosAsync(partida,recursos.Felicidad, recursos.Contaminacion, recursos.EcoCoins, recursos .Energia);
         await _partidaRepositorio.Actualizar(partida);
        
        await this._uow.CommitAsync();

        recursoAdevolver = await ObtenerRecursos(partida.Id); 
        return recursoAdevolver; 
        
    }
}
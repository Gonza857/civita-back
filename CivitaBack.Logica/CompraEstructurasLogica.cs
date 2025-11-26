using CivitaBack.Domain.Entidades;
using CivitaBack.Domain.Excepciones;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Domain.Interfaces.Repositorios;
using CivitaBack.Logica.Interfaces;
using CivitaBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Logica
{
    public class CompraEstructurasLogica : ICompraEstructurasLogica
    {
        private readonly IPartidaRepositorio _repositorioPartida;
        private readonly IEstructuraRepositorio _estructuraRepositorio;
        private readonly IAccesoUsuarios _accesoUsuarios;
        private readonly IActualizarRecursosLogica _actualizarRecursosLogica;
        private readonly IUnidadDeTrabajo _uow;

        public CompraEstructurasLogica(
            IPartidaRepositorio repositorioPartida,
            IEstructuraRepositorio estructuraRepositorio,
            IAccesoUsuarios accesoUsuarios,
            IActualizarRecursosLogica actualizarRecursosLogica,
            IUnidadDeTrabajo uow)
        {
            _repositorioPartida = repositorioPartida;
            _estructuraRepositorio = estructuraRepositorio;
            _accesoUsuarios = accesoUsuarios;
            _actualizarRecursosLogica = actualizarRecursosLogica;
            _uow = uow;
        }

        public async Task<int> ComprarEstructuraAsync(int partidaId, int estructuraId)
        {
            const int MAX_REINTENTOS = 3;
            
            Estructura? estructura = await _estructuraRepositorio.ObtenerPorId(estructuraId);

            if (estructura == null)
                throw new PartidaExcepcion("Estructura no encontrada.");
            
            for (int intento = 0; intento < MAX_REINTENTOS; intento++)
            {
                try
                {
                    Partida? partida = await _repositorioPartida.ObtenerPorIdTrackeada(partidaId);

                    if (partida == null || partida.Recursos == null)
                        throw new PartidaExcepcion("Partida inválida o recursos no encontrados.");
                    
                    int costo = estructura.CostoDinero;

                    if (partida.Recursos.EcoCoins < costo)
                        throw new PartidaExcepcion("Dinero insuficiente.");

                    int cambioEcoCoins = -costo;
                    
                    _actualizarRecursosLogica.ActualizarRecursosAsync(partida, 0, 0, cambioEcoCoins, 0);
                    
                    _repositorioPartida.SincronizarCambios(partida);
                    
                    await _uow.CommitAsync();
                    
                    return partida.Recursos.EcoCoins;
                }
                catch (DbUpdateConcurrencyException ex)
                {

                    if (intento == MAX_REINTENTOS - 1)
                    {
                        throw new PartidaExcepcion("Error de concurrencia. Intente de nuevo.");
                    }
                    
                    await Task.Delay(50);
                }
            }

            // Debería ser inalcanzable, pero lo dejamos por seguridad.
            throw new InvalidOperationException("Falló la operación después de todos los reintentos.");
        }
    }
}
using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace CivitaBack.Logica.Backgrounds
{
    public class BackgroundCicloLogica : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundCicloLogica> _logger;
        private readonly IHubContext<CicloHub> _hubContext;
        private readonly ManualResetEventSlim _pauseEvent = new(true); // empieza "activo"


        private readonly TimeSpan _intervalo = TimeSpan.FromSeconds(5); // 7/8

        public BackgroundCicloLogica(IServiceProvider serviceProvider, ILogger<BackgroundCicloLogica> logger,
            IHubContext<CicloHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hubContext = hubContext;
        }

        public void Pausar() => _pauseEvent.Reset();
        public void Continuar() => _pauseEvent.Set();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🟢 CicloBackgroundService iniciado a las {Hora}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 1. Espera si está en pausa
                    _pauseEvent.Wait(stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var cicloLogica = scope.ServiceProvider.GetRequiredService<ICicloLogica>();

                    // 2. ¡HACE EL TRABAJO! (Esto ahora se ejecuta primero)
                    var partidas = await cicloLogica.EjecutarCicloAsync();

                    // 3. Enviar los recursos a cada grupo de SignalR
                    foreach (var partida in partidas)
                    {
                        var payload = new RecursoDTO
                        {
                            Energia = partida.Recursos.Energia,
                            Contaminacion = partida.Recursos.Contaminacion,
                            Felicidad = partida.Recursos.Felicidad,
                            EcoCoins = partida.Recursos.EcoCoins,
                            Poblacion = partida.Recursos.Poblacion
                        };

                        await _hubContext.Clients.Group(partida.Id.ToString())
                            .SendAsync("RecursosActualizados", payload);
                    }

                    _logger.LogInformation("✅ Ciclo ejecutado y recursos enviados a SignalR a las {Hora}",
                        DateTime.Now);

                    // 4. ¡ESPERA DESPUÉS de terminar el trabajo!
                    await Task.Delay(_intervalo, stoppingToken);
                }
                catch (Exception ex)
                {
                    // 5. Si algo falló (en EjecutarCicloAsync o SignalR), loguealo
                    _logger.LogError(ex, "❌ Error durante la ejecución del ciclo automático");

                    // 6. Esperá 15 segundos antes de reintentar el ciclo
                    //    (para no spamear la BD si el error es grave)
                    await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                }
            }

            _logger.LogInformation("🛑 CicloBackgroundService detenido a las {Hora}", DateTime.Now);
        }
    }
}
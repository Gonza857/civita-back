using CivitaBack.Data.BO;
using CivitaBack.Data.DTO;
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
        private readonly TimeSpan _intervalo = TimeSpan.FromSeconds(15);

        public BackgroundCicloLogica(IServiceProvider serviceProvider, ILogger<BackgroundCicloLogica> logger, IHubContext<CicloHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🟢 CicloBackgroundService iniciado a las {Hora}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var cicloLogica = scope.ServiceProvider.GetRequiredService<ICicloLogica>();

                    // Espera hasta el próximo ciclo
                    await Task.Delay(_intervalo, stoppingToken);

                    var partidas = await cicloLogica.EjecutarCicloAsync();

                    // Enviar los recursos a cada grupo de SignalR
                    foreach (var partida in partidas) { 
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

                    _logger.LogInformation("✅ Ciclo ejecutado y recursos enviados a SignalR a las {Hora}", DateTime.Now);
            }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error durante la ejecución del ciclo automático");
                    
                    await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken); 
                }

            }

            _logger.LogInformation("🛑 CicloBackgroundService detenido a las {Hora}", DateTime.Now);
        }

    }
}

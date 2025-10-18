using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace CivitaBack.Logica.Backgrounds
{
    public class BackgroundCicloLogica : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundCicloLogica> _logger;
        private readonly TimeSpan _intervalo = TimeSpan.FromSeconds(15);

        public BackgroundCicloLogica(IServiceProvider serviceProvider, ILogger<BackgroundCicloLogica> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🟢 CicloBackgroundService iniciado a las {Hora}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Crear el servicio scoped manualmente
                        var cicloLogica = scope.ServiceProvider.GetRequiredService<ICicloLogica>();
                        
                        // Espera hasta el próximo ciclo
                        await Task.Delay(_intervalo, stoppingToken);
                        
                        await cicloLogica.EjecutarCicloAsync();
                        _logger.LogInformation("✅ Ciclo ejecutado automáticamente a las {Hora}", DateTime.Now);

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error durante la ejecución del ciclo automático");
                }

            }

            _logger.LogInformation("🛑 CicloBackgroundService detenido a las {Hora}", DateTime.Now);
        }

    }
}

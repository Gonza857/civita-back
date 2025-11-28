using CivitaBack.Data.DTO;
using CivitaBack.Domain.Interfaces.Logica;
using CivitaBack.Logica.Hubs;
using CivitaBack.Logica.Interfaces;
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

        private readonly TimeSpan _intervalo = TimeSpan.FromSeconds(10); // 7/8

        public BackgroundCicloLogica(IServiceProvider serviceProvider, ILogger<BackgroundCicloLogica> logger,
            IHubContext<CicloHub> hubContext)
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
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        _pauseEvent.Wait(stoppingToken);

                        var cicloLogica = scope.ServiceProvider.GetRequiredService<ICicloLogica>();
                        var eventoLogica = scope.ServiceProvider.GetRequiredService<IEventoLogica>();
                        var nivelLogica = scope.ServiceProvider.GetRequiredService<INivelLogica>();

                        var partidas = await cicloLogica.EjecutarCicloAsync();

                        foreach (var partida in partidas)
                        {
                            if (partida.EstaPausada)
                                continue;

                            if (partida == null || partida.Recursos == null)
                                continue;

                            var payload = new RecursoDTO
                            {
                                Energia = partida.Recursos.Energia,
                                Contaminacion = partida.Recursos.Contaminacion,
                                Felicidad = partida.Recursos.Felicidad,
                                EcoCoins = partida.Recursos.EcoCoins,
                                Poblacion = partida.Recursos.Poblacion,
                                Nivel = partida.Nivel,
                                Experiencia = partida.Experiencia,
                                ExperienciaSiguienteNivel = nivelLogica.ObtenerExperienciaTechoNivel(partida.Nivel)
                            };

                            if (partida.Recursos.Contaminacion > 80)
                            {
                                var tipDisparado = await eventoLogica.DispararEventoInformativoAsync(partida, 2);
                            
                                if (tipDisparado != null)
                                {
                                    await _hubContext.Clients.Group(partida.Id.ToString())
                                        .SendAsync("EventoDisparado", tipDisparado);
                                }
                            }

                            if (partida.Recursos.Energia < 10)
                            {
                                var tipDisparado = await eventoLogica.DispararEventoInformativoAsync(partida, 3);

                                if (tipDisparado != null)
                                {
                                    await _hubContext.Clients.Group(partida.Id.ToString())
                                        .SendAsync("EventoDisparado", tipDisparado);
                                }
                            }

                            if (partida.Recursos.Felicidad < 25)
                            {
                                var tipDisparado = await eventoLogica.DispararEventoInformativoAsync(partida, 4);

                                if (tipDisparado != null)
                                {
                                    await _hubContext.Clients.Group(partida.Id.ToString())
                                        .SendAsync("EventoDisparado", tipDisparado);
                                }
                            }

                            await _hubContext.Clients.Group(partida.Id.ToString())
                                .SendAsync("RecursosActualizados", payload);
                        }

                        _logger.LogInformation("✅ Ciclo ejecutado y recursos enviados a SignalR a las {Hora}",
                            DateTime.Now);

                        await Task.Delay(_intervalo, stoppingToken);
                    }
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
using CivitaBack.Data.Repositorio;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace CivitaBack.Logica.Backgrounds
{
    /*public class BackgroundEventoLogica : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundEventoLogica> _logger;
        private readonly TimeSpan _intervalo = TimeSpan.FromMinutes(20);

        public BackgroundEventoLogica(IServiceProvider serviceProvider, ILogger<BackgroundEventoLogica> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🟢 BackgroundEventoService iniciado a las {Hora}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();

                    var eventoLogica = scope.ServiceProvider.GetRequiredService<IEventoLogica>();
                    var partidaRepositorio = scope.ServiceProvider.GetRequiredService<IPartidaRepositorio>();

                    await Task.Delay(_intervalo, stoppingToken);

                    var partidas = await partidaRepositorio.ObtenerPartidas();

                    foreach (var partida in partidas)
                    {
                        //if (!partida.UltimaVez.HasValue) continue;

                        //var tiempoTranscurrido = (DateTime.UtcNow - partida.UltimaVez.Value).TotalMinutes;

                        _logger.LogInformation("💡 Disparando evento para partida {PartidaId}", partida.Id);

                        var evento = await eventoLogica.DispararEventoAsync(partida.Id);

                        _logger.LogInformation("🎉 Evento disparado para partida {PartidaId}: {DescripcionEvento}",
                            partida.Id, evento.TextoDescripcion);

                        if (evento != null)
                        {
                            _logger.LogInformation("Evento {EventoId} disparado: {Descripcion}", evento.Id, evento.TextoDescripcion);
                        }

                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en BackgroundEventoService");
                }

                _logger.LogInformation("🛑 BackgroundEventoService detenido a las {Hora}", DateTime.Now);

            }
        }
    }*/
}

using Microsoft.Extensions.Hosting;

namespace Blazor.Status.Backend.Services
{
    public class StatusBackgroundService : BackgroundService
    {

        private readonly ServerPingService _serverPingService;

        public StatusBackgroundService(
            ServerPingService serverPingService)
        {
            _serverPingService = serverPingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _serverPingService.CheckPings();
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

    }
}

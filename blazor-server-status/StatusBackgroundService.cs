using blazor_server_status.Data;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

namespace blazor_server_status
{
    public class StatusBackgroundService : BackgroundService
    {

        private readonly ILogger<StatusBackgroundService> _logger;
        private readonly ServerPingService _serverPingService;

        public StatusBackgroundService(ILogger<StatusBackgroundService> logger,
            ServerPingService serverPingService)
        {
            _logger = logger;
            _serverPingService = serverPingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _serverPingService.CheckPings();
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

    }
}

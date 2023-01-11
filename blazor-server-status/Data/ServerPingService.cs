using blazor_server_status.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

namespace blazor_server_status.Data
{
    public class ServerPingService
    {
        private readonly List<ServerModel> ServerList;

        private readonly RedisService _redisService;

        private readonly IHubContext<ServerHub> _hubContext;

        public ServerPingService(IOptions<ApiConfig> apiConfig,
            RedisService redisService,
            IHubContext<ServerHub> hubContext)
        {
            ServerList = apiConfig.Value.Servers.ToList();
            _hubContext = hubContext;
            _redisService = redisService;
        }

        public List<string> GetLogsByHost(string host)
        {
            return _redisService.Get<List<string>>(host);
        }

        public async Task CheckPings()
        {
            for (var i = 0; i < ServerList.Count(); i++)
            {
                var item = ServerList[i];
                if (item.Enabled == false)
                {
                    return;
                }
                if (item.IsExecuteTime())
                {
                    var isOnline = NetUtility.Ping(item.Host, item.Port, TimeSpan.FromSeconds(5));
                    if (isOnline)
                    {
                        item.WriteLog("{0} Server is online");
                        return;
                    }
                    item.WriteLog("{0} Server is offline");
                }

                _redisService.Set(item.Host, item.Logs, TimeSpan.FromHours(24));
                ServerList[i] = item;
            }
            await _hubContext.Clients.All.SendAsync("NotifyServerChange", JsonSerializer.Serialize(ServerList));
        }
    }
}
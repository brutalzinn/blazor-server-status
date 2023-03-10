using Blazor.Status.Backend.Configs;
using Blazor.Status.Backend.Hubs;
using Blazor.Status.Backend.Models;
using Blazor.Status.Backend.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

namespace Blazor.Status.Backend.Services
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

        public List<LogModel> GetServerInfo(string host)
        {
            return _redisService.Get<List<LogModel>>(host);
        }

        public async Task CheckPings()
        {
            for (var i = 0; i < ServerList.Count(); i++)
            {
                var item = ServerList[i];
                if (item.Enabled && item.IsExecuteTime())
                {
                    var isOnline = NetUtility.Ping(item.Host, item.Port, TimeSpan.FromSeconds(5));
                    item.Status = isOnline ? ServerStatus.ONLINE : ServerStatus.OFFLINE;
                    item.UpdateStatusChange();
                    _redisService.Set(item.Host, item.Logs, TimeSpan.FromDays(7));
                }
                ServerList[i] = item;
            }
            await _hubContext.Clients.All.SendAsync("NotifyServerChange", JsonSerializer.Serialize(ServerList));
        }
    }
}
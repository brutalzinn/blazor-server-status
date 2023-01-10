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

        private readonly IHubContext<ServerHub> _hubContext;

        public ServerPingService(IOptions<ApiConfig> apiConfig, IHubContext<ServerHub> hubContext)
        {
            ServerList = apiConfig.Value.Servers.ToList();
            _hubContext = hubContext;
        }

        public async Task<List<ServerModel>> GetServerList()
        {
            return ServerList;
        }

        public async Task CheckPings()
        {
            for (var i = 0; i < ServerList.Count(); i++)
            {
                var item = ServerList[i];
                if (item.IsExecuteTime())
                {
                    item.IsOnline = PingHost(item.Host, item.Port);
                }
                ServerList[i] = item;
            }
            await _hubContext.Clients.All.SendAsync("NotifyServerChange", JsonSerializer.Serialize(ServerList));
        }

        public static bool PingHost(string hostUri, int? portNumber)
        {

            using var ping = new Ping();
            PingReply res = ping.Send(hostUri);
            return res.Status == IPStatus.Success;
        }
    }
}
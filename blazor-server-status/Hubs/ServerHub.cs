using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace blazor_server_status.Hubs
{
    public class ServerHub : Hub
    {
        public async Task SendMessage(List<ServerModel> serverStatus)
        {
            Clients.All.SendAsync("NotifyServerChange", serverStatus);
        }
    }
}

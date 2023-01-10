using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace blazor_server_status.Hubs
{
    public class ServerHub : Hub
    {
        public async Task SendMessage(List<ServerModel> serverStatus)
        {
            await Clients.All.SendAsync("NotifyServerChange", JsonSerializer.Serialize(serverStatus));
        }
    }
}

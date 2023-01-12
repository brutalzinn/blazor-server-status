using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace blazor_server_status.Hubs
{
    /// <summary>
    /// i dont  need this typed now.
    /// </summary>
    public class ServerHub : Hub
    {
        //public async Task SendMessage(List<ServerModel> serverStatus)
        //{
        //    await Clients.All.SendAsync("NotifyServerChange", serverStatus);
        //}
    }
}

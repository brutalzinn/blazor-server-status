using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Blazor.Status.Backend.Hubs
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

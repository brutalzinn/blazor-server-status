using Blazor.Status.Backend.Models;

namespace Blazor.Status.Backend.Application.Messages.Hubs.ServerHub
{
    public class ServerMessageModel
    {
        public string? Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public TimeSpan ExecuteIn { get; set; }
        public bool Enabled { get; set; }
        public DateTime LastCheck { get; set; }
        public DateTime NextCheck { get; set; }
        public DateTime LastUpdate { get; set; }
        public ServerStatus Status { get; set; }
        public List<LogModel> Logs { get; set; }
        public string MessageStatus => Status == ServerStatus.ONLINE ? "Online" : "Offline";

        public ServerMessageModel()
        {
            if (Name is null)
            {
                Name = Host;
            }
        }
    }
}

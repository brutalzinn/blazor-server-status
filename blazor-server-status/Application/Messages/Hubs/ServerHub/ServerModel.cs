namespace blazor_server_status.Application.Messages.Hubs.ServerHub
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
        public bool IsOnline { get; set; }
        public List<string> Logs { get; private set; }

        public ServerMessageModel()
        {
            if (Name is null)
            {
                Name = Host;
            }
            Logs = new List<string>();
        }
    }
}

namespace blazor_server_status
{
    public class ApiConfig
    {
        public IEnumerable<ServerModel> Servers { get; set; }
    }
    public class ServerModel
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

        public ServerModel()
        {
            Logs = new List<string>();
        }
        public bool IsExecuteTime()
        {
            if (DateTime.Now >= NextCheck)
            {
                LastCheck = DateTime.Now;
                UpdateNextCheck();
                return true;
            }
            LastCheck = DateTime.Now;
            return false;
        }
        public void UpdateNextCheck()
        {
            NextCheck = LastCheck.Add(ExecuteIn);
            RefreshLastUpdate();
        }
        public void WriteLog(string log)
        {
            var onlineStatus = IsOnline ? "Online" : "Offline";

            var lastLog = Logs.LastOrDefault();
            if (lastLog != null && lastLog.EndsWith("-" + onlineStatus))
            {
                return;
            }
            Logs.Add(string.Format("{0} {1}-{2}", log, DateTime.Now, onlineStatus));
        }
        private void RefreshLastUpdate()
        {
            LastUpdate = DateTime.Now;
        }
    }
}

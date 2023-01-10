namespace blazor_server_status
{
    public class ApiConfig
    {
        public IEnumerable<ServerModel> Servers { get; set; }
    }
    public class ServerModel
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public TimeSpan ExecuteIn { get; set; }
        public bool Enabled { get; set; }
        public DateTime LastCheck { get; set; }
        public DateTime NextCheck { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsOnline { get; set; }
        public List<string> Logs { get; private set; }

        public bool IsExecuteTime()
        {
            if (DateTime.Now >= NextCheck)
            {
                UpdateNextCheck();
                LastCheck = DateTime.Now;
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
        public void InsertOfflineLog()
        {
            Logs.Add("Server is offline");
            RefreshLastUpdate();

        }
        public void InsertOnlineLog()
        {
            Logs.Add("Server is online");
            RefreshLastUpdate();

        }
        public void ClearLogs()
        {
            Logs.Clear();
            RefreshLastUpdate();
        }
        private void RefreshLastUpdate()
        {
            LastUpdate = DateTime.Now;
        }
    }
}

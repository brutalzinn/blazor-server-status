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
            if (Name == null)
            {
                Name = Host;
            }
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
            Logs.Add(string.Format(log, DateTime.Now));
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

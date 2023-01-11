using blazor_server_status.Models;

namespace blazor_server_status.Infra
{
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
        public ServerStatus Status { get; set; }
        public List<LogModel> Logs { get; private set; }
        private string MessageStatus => Status == ServerStatus.ONLINE ? "Online" : "Offline";

        public ServerModel()
        {
            Logs = new List<LogModel>();
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

        public void UpdateStatusChange()
        {
            WriteLog($"{Name} is {MessageStatus}");
        }
        private void WriteLog(string log)
        {
            var lastLog = Logs.LastOrDefault();
            if (lastLog.Status.Equals(Status))
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

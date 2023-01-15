using blazor_server_status.Application.Messages.Hubs.ServerHub;
using blazor_server_status.Models;

namespace blazor_server_status.Models
{
    public class ServerModel
    {
        public string? Name
        {
            get
            {
                return _name ?? Host;
            }

            set
            {
                _name = value;
            }
        }
        public string Host { get; set; }
        public int Port { get; set; }
        public TimeSpan ExecuteIn { get; set; }
        public bool Enabled { get; set; }
        public DateTime LastCheck { get; set; }
        public DateTime NextCheck { get; set; }
        public DateTime LastUpdate { get; set; }
        public ServerStatus Status { get; set; }
        public List<LogModel> Logs { get; private set; }
        public string MessageStatus => Status == ServerStatus.ONLINE ? "Online" : "Offline";
        private string _name { get; set; }

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
            //temporary solution because in Brazil we use common DD/MM/YYYY HH:TT:SS format for dates with time.
            WriteLog(string.Format("{0}-{1} is {0}", DateTime.Now.ToString("g"), Name, MessageStatus));
        }
        private void WriteLog(string log)
        {
            var lastLog = Logs.LastOrDefault();
            if (lastLog != null && lastLog.Status.Equals(Status))
            {
                return;
            }
            Logs.Add(new LogModel(log, Status));
        }
        private void RefreshLastUpdate()
        {
            LastUpdate = DateTime.Now;
        }

        public ServerMessageModel ToMap()
        {
            return new ServerMessageModel()
            {
                Host = Host,
                Enabled = Enabled,
                ExecuteIn = ExecuteIn,
                LastCheck = LastCheck,
                LastUpdate = LastUpdate,
                Logs = Logs,
                Name = Name,
                NextCheck = NextCheck,
                Port = Port,
                Status = Status
            };
        }
    }
}

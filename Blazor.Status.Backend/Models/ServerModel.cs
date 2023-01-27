using Blazor.Status.Backend.Application.Messages.Hubs.ServerHub;
using Serilog;

namespace Blazor.Status.Backend.Models
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
            LastCheck = DateTime.Now;
            if (DateTime.Now >= NextCheck)
            {
                UpdateNextCheck();
                return true;
            }
            return false;
        }
        public void UpdateNextCheck()
        {
            NextCheck = LastCheck.Add(ExecuteIn);
            RefreshLastUpdate();
        }

        public void UpdateStatusChange()
        {
            /// i need to check a correct form to do this here. This is because the current culture .use only configures builder steps.
            /// But the services sitill dont know about pt-BR is default.
            var currentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            WriteLog($"{currentDateTime}-{Name} is {MessageStatus}");
        }
        private void WriteLog(string log)
        {
            var lastLog = Logs.LastOrDefault();
            if (lastLog != null && lastLog.Status.Equals(Status))
            {
                return;
            }

            Log.Warning(log);
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

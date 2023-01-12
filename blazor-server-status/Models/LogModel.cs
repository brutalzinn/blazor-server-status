namespace blazor_server_status.Models
{
    public class LogModel
    {

        public string Message { get; set; }
        public ServerStatus Status { get; set; }
        public DateTime LogTime { get; set; }

        public LogModel(string message, ServerStatus status)
        {
            Message = message;
            Status = status;
            LogTime = DateTime.Now;
        }


    }
}

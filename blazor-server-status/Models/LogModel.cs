namespace blazor_server_status.Models
{
    public class LogModel
    {
        public string Message { get; set; }
        public ServerStatus Status { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}

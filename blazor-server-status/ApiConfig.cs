using blazor_server_status.Models;

namespace blazor_server_status
{
    public class ApiConfig
    {
        public IEnumerable<ServerModel> Servers { get; set; }
    }

}

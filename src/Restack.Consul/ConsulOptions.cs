using Consul;

namespace Restack.Consul
{
    public class ConsulOptions
    {
        public string ServiceName { get; set; }
        public ConsulClient Client { get; set; }
    }
}

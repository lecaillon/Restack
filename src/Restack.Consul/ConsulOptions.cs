using Consul;

namespace Restack.Consul
{
    public class ConsulOptions
    {
        public ConsulClient ConsulClient { get; set; } = new ConsulClient();
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Restack.Consul
{
    public interface IServiceDiscoveryClient
    {
        Task<Uri> GetUriAsync(string serviceName);

        Task<Uri> GetUriAsync(string serviceName, CancellationToken token);
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Restack.Consul
{
    public interface IServiceDiscoveryClient
    {
        Task<Uri> GetUriAsync(Uri service);

        Task<Uri> GetUriAsync(Uri service, CancellationToken token);
    }
}

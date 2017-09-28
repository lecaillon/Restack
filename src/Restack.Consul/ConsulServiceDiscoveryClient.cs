using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Restack.Consul
{
    public class ConsulServiceDiscoveryClient : IServiceDiscoveryClient
    {
        public async Task<Uri> GetUriAsync(Uri service)
        {
            return await GetUriAsync(service, default(CancellationToken));
        }

        public Task<Uri> GetUriAsync(Uri service, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}

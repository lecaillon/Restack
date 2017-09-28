using System;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Options;
using Preconditions;

namespace Restack.Consul
{
    public class ConsulServiceDiscoveryClient : IServiceDiscoveryClient
    {
        private readonly ConsulClient _consulClient;

        public ConsulServiceDiscoveryClient(IOptions<ConsulOptions> consulOptions)
        {
            Check.NotNull(consulOptions, nameof(consulOptions));

            _consulClient = consulOptions.Value.ConsulClient ?? throw new ArgumentNullException("ConsulClient");
        }

        public async Task<Uri> GetUriAsync(string serviceName)
        {
            return await GetUriAsync(serviceName, default(CancellationToken));
        }

        public Task<Uri> GetUriAsync(string serviceName, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Preconditions;
using Restack.Http;

namespace Restack.Consul
{
    internal class ConsulHttpClientFactoryPolicy : MessageHandlerHttpClientFactoryPolicy
    {
        private readonly IServiceDiscoveryClient _discoveryClient;
        private readonly IOptionsMonitor<ServiceDiscoveryOptions> _serviceDiscoveryOptions;

        public ConsulHttpClientFactoryPolicy(IServiceDiscoveryClient discoveryClient, IOptionsMonitor<ServiceDiscoveryOptions> serviceDiscoveryOptions)
        {
            _discoveryClient = Check.NotNull(discoveryClient, nameof(discoveryClient));
            _serviceDiscoveryOptions = Check.NotNull(serviceDiscoveryOptions, nameof(serviceDiscoveryOptions));
        }

        public override int Order => 2000;

        protected override DelegatingHandler CreateHandler(HttpClientFactoryContext context)
        {
            return new ConsulMessageHandler(_discoveryClient, _serviceDiscoveryOptions, context.Name);
        }
 
        private class ConsulMessageHandler : DelegatingHandler
        {
            private readonly IServiceDiscoveryClient _discoveryClient;
            private readonly IOptionsMonitor<ServiceDiscoveryOptions> _serviceDiscoveryOptions;
            private readonly string _clientName;

            public ConsulMessageHandler(IServiceDiscoveryClient discoveryClient, IOptionsMonitor<ServiceDiscoveryOptions> serviceDiscoveryOptions, string clientName)
            {
                _discoveryClient = discoveryClient;
                _serviceDiscoveryOptions = serviceDiscoveryOptions;
                _clientName = clientName;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var options = _serviceDiscoveryOptions.Get(_clientName);
                if(options != null)
                {
                    request.RequestUri = await _discoveryClient.GetUriAsync(options.ServiceName, cancellationToken);
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}

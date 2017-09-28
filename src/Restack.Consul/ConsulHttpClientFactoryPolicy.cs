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
        private readonly IOptionsMonitor<ConsulOptions> _options;
        private readonly IServiceDiscoveryClient _discoveryClient;

        public ConsulHttpClientFactoryPolicy(IServiceDiscoveryClient discoveryClient, IOptionsMonitor<ConsulOptions> options)
        {
            _options = Check.NotNull(options, nameof(options));
            _discoveryClient = Check.NotNull(discoveryClient, nameof(discoveryClient));
        }

        public override int Order => 2000;

        protected override DelegatingHandler CreateHandler(HttpClientFactoryContext context)
        {
            return new ConsulMessageHandler(_discoveryClient, _options, context.Name);
        }
 
        private class ConsulMessageHandler : DelegatingHandler
        {
            private readonly IServiceDiscoveryClient _discoveryClient;
            private readonly IOptionsMonitor<ConsulOptions> _options;
            private readonly string _clientName;

            public ConsulMessageHandler(IServiceDiscoveryClient discoveryClient, IOptionsMonitor<ConsulOptions> optionsFactory, string clientName)
            {
                _discoveryClient = discoveryClient;
                _options = optionsFactory;
                _clientName = clientName;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var options = _options.Get(_clientName);
                if(options != null)
                {
                    request.RequestUri = await _discoveryClient.GetUriAsync(request.RequestUri);
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}

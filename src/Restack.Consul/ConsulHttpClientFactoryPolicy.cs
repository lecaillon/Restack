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

        public ConsulHttpClientFactoryPolicy(IServiceDiscoveryClient discoveryClient)
        {
            _discoveryClient = Check.NotNull(discoveryClient, nameof(discoveryClient));
        }

        public override int Order => 2000;

        protected override DelegatingHandler CreateHandler(HttpClientFactoryContext context)
        {
            return new ConsulMessageHandler(_discoveryClient, context.Name);
        }
 
        private class ConsulMessageHandler : DelegatingHandler
        {
            private readonly IServiceDiscoveryClient _discoveryClient;
            private readonly string _clientName;

            public ConsulMessageHandler(IServiceDiscoveryClient discoveryClient, string clientName)
            {
                _discoveryClient = discoveryClient;
                _clientName = clientName;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.RequestUri = await _discoveryClient.GetUriAsync(request.RequestUri, cancellationToken);
                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}

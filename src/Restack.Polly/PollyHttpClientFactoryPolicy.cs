using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Preconditions;

namespace Restack.Polly
{
    internal class PollyHttpClientFactoryPolicy : MessageHandlerHttpClientFactoryPolicy
    {
        private readonly IOptionsMonitor<PollyOptions> _options;

        public PollyHttpClientFactoryPolicy(IOptionsMonitor<PollyOptions> options)
        {
            _options = Check.NotNull(options, nameof(options));
        }

        public override int Order => 0;

        protected override DelegatingHandler CreateHandler(HttpClientFactoryContext context)
        {
            return new PollyMessageHandler(_options, context.Name);
        }

        internal class PollyMessageHandler : DelegatingHandler
        {
            private readonly IOptionsMonitor<PollyOptions> _options;
            private readonly string _name;

            public PollyMessageHandler(IOptionsMonitor<PollyOptions> optionsFactory, string name)
            {
                _options = optionsFactory;
                _name = name;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var options = GetOptions(request);
                if (options.Policies.Count == 0)
                {
                    return await base.SendAsync(request, cancellationToken);
                }

                return await options.Policies.Combine().ExecuteAsync(t => base.SendAsync(request, t), cancellationToken);
            }

            private PollyOptions GetOptions(HttpRequestMessage request)
            {
                if (_name != HttpClientFactory.DefaultName)
                {
                    return _options.Get(_name);
                }

                if (request.RequestUri.HostNameType == UriHostNameType.Dns)
                {
                    return _options.Get(request.RequestUri.Host);
                }

                return null;
            }
        }
    }
}

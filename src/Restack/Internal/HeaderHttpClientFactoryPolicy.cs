using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Preconditions;

namespace Restack.Internal
{
    internal class HeaderHttpClientFactoryPolicy : IHttpClientFactoryPolicy
    {
        private readonly IOptionsMonitor<HeaderOptions> _options;

        public HeaderHttpClientFactoryPolicy(IOptionsMonitor<HeaderOptions> options)
        {
            _options = Check.NotNull(options, nameof(options));
        }

        public int Order => 1000;

        public void Apply(HttpClientFactoryContext context)
        {
            Check.NotNull(context, nameof(context));

            context.PrependMessageHandler(new DefaultHeaderMessageHandler(_options, context.Name));
        }

        private class DefaultHeaderMessageHandler : DelegatingHandler
        {
            private IOptionsMonitor<HeaderOptions> _options;
            private string _name;

            public DefaultHeaderMessageHandler(IOptionsMonitor<HeaderOptions> options, string name)
            {
                _options = options;
                _name = name;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var options = GetOptions(request);
                foreach (var kvp in options.Headers)
                {
                    if (!request.Headers.TryAddWithoutValidation(kvp.Key, (IEnumerable<string>)kvp.Value))
                    {
                        request.Content?.Headers.TryAddWithoutValidation(kvp.Key, (IEnumerable<string>)kvp.Value);
                    }
                }

                return base.SendAsync(request, cancellationToken);
            }

            private HeaderOptions GetOptions(HttpRequestMessage request)
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

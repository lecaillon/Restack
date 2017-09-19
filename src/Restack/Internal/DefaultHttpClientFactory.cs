using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Preconditions;

namespace Restack.Internal
{
    internal class DefaultHttpClientFactory : HttpClientFactory
    {
        private readonly IHttpClientFactoryPolicy[] _policies;

        public DefaultHttpClientFactory() : this(Array.Empty<IHttpClientFactoryPolicy>()) { }

        public DefaultHttpClientFactory(IEnumerable<IHttpClientFactoryPolicy> policies)
        {
            Check.NotNull(policies, nameof(policies));

            _policies = policies.OrderBy(p => p.Order).ToArray();

            ClientHandler = new HttpClientHandler();
        }

        public HttpMessageHandler ClientHandler { get; set; }

        public override HttpClient GetClient(string name)
        {
            Check.NotNull(name, nameof(name));

            var context = new Context(name) { MessageHandler = ClientHandler };
            for (var i = 0; i < _policies.Length; i++)
            {
                _policies[i].Apply(context);
            }

            return new HttpClient(context.MessageHandler)
            {
                BaseAddress = context.BaseAddress
            };
        }

        private class Context : HttpClientFactoryContext
        {
            public Context(string name)
            {
                Name = name;
            }

            public override Uri BaseAddress { get; set; }

            public override string Name { get; }

            public override HttpMessageHandler MessageHandler { get; set; }

            public override void PrependMessageHandler(DelegatingHandler messageHandler)
            {
                Check.NotNull(messageHandler, nameof(messageHandler));

                messageHandler.InnerHandler = MessageHandler;
                MessageHandler = messageHandler;
            }
        }
    }

}

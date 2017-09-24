using System.Net.Http;
using Preconditions;

namespace Restack.Http
{
    public abstract class MessageHandlerHttpClientFactoryPolicy : IHttpClientFactoryPolicy
    {
        public abstract int Order { get; }

        public void Apply(HttpClientFactoryContext context)
        {
            Check.NotNull(context, nameof(context));

            var handler = CreateHandler(context);
            handler.InnerHandler = context.MessageHandler;
            context.MessageHandler = handler;
        }

        protected abstract DelegatingHandler CreateHandler(HttpClientFactoryContext context);
    }
}

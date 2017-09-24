using Microsoft.Extensions.Options;
using Preconditions;

namespace Restack.Http
{
    internal class OptionsHttpClientFactoryPolicy : IHttpClientFactoryPolicy
    {
        private readonly IOptionsMonitor<HttpClientOptions> _options;

        public OptionsHttpClientFactoryPolicy(IOptionsMonitor<HttpClientOptions> options)
        {
            _options = Check.NotNull(options, nameof(options));
        }

        public int Order => 1000;

        public void Apply(HttpClientFactoryContext context)
        {
            Check.NotNull(context, nameof(context));

            if (context.Name == HttpClientFactory.DefaultName)
            {
                return;
            }

            var options = _options.Get(context.Name);
            context.BaseAddress = options.BaseAddress;
            if(options.MessageHandler != null)
            {
                context.MessageHandler = options.MessageHandler;
            }
        }
    }
}

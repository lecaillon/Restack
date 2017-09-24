using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Preconditions;

namespace Restack.Http
{
    internal class LoggingHttpClientFactoryPolicy : MessageHandlerHttpClientFactoryPolicy
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggingHttpClientFactoryPolicy(ILoggerFactory loggerFactory)
        {
            _loggerFactory = Check.NotNull(loggerFactory, nameof(loggerFactory));
        }

        public override int Order => int.MinValue + 1000;

        protected override DelegatingHandler CreateHandler(HttpClientFactoryContext context) => new MessageHandler(CreateLogger(context.Name));

        private ILogger CreateLogger(string clientName) => _loggerFactory.CreateLogger("Restack.Http." + clientName);

        private class MessageHandler : DelegatingHandler
        {
            private readonly ILogger _logger;

            public MessageHandler(ILogger logger)
            {
                _logger = logger;
            }

            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Sending {HttpMethod} request to {RequestUri}", request.Method, request.RequestUri);
                var response = await base.SendAsync(request, cancellationToken);
                _logger.LogInformation("Response was a {Status}", response.StatusCode);

                return response;
            }
        }
    }
}

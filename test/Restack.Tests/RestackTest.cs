using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Restack.Tests
{
    public class RestackTest
    {
        [Fact(DisplayName = "HttpClientFactory_works")]
        public void HttpClientFactory_works()
        {
            var serviceProvider = new ServiceCollection().AddOptions()
                                                         .AddRestack()
                                                         .BuildServiceProvider();

            var clientFactory = serviceProvider.GetRequiredService<HttpClientFactory>();
            var httpClient = clientFactory.GetDefaultClient();

            httpClient.Should().NotBeNull();
        }
    }
}

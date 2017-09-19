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
            // Arrange
            var serviceProvider = new ServiceCollection().AddOptions()
                                                         .AddRestack()
                                                         .BuildServiceProvider();

            var clientFactory = serviceProvider.GetRequiredService<HttpClientFactory>();

            // Act
            var httpClient = clientFactory.GetDefaultClient();

            // Assert
            httpClient.Should().NotBeNull();
        }

        [Fact(DisplayName = "Multi_header_values_work")]
        public async void Multi_header_values_work()
        {
            // Arrange
            var serviceProvider = new ServiceCollection().AddOptions()
                                                         .AddRestack()
                                                         .AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent"))
                                                         .BuildServiceProvider();

            var clientFactory = serviceProvider.GetRequiredService<HttpClientFactory>();

            // Act
            var response = await clientFactory.GetDefaultClient().GetAsync("https://google.com/");

            // Assert
            response.RequestMessage.Headers.UserAgent.ToString().Should().Be("myagent");
        }
    }
}

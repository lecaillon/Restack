using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
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

        [Theory(DisplayName = "HttpClientFactory_with_multi_header_values_work")]
        [MemberData(nameof(InitHttpClientFactory))]
        public async void HttpClientFactory_with_multi_header_values_work(HttpClientFactory clientFactory)
        {
            // Act
            var response = await clientFactory.GetDefaultClient().GetAsync("https://google.com/");

            // Assert
            response.RequestMessage.Headers.UserAgent.ToString().Should().Be("myagent");
        }

        private static IEnumerable<HttpClientFactory[]> InitHttpClientFactory()
        {
            yield return new HttpClientFactory[]
            {
                new ServiceCollection().AddOptions()
                                       .AddRestack()
                                       .AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent"))
                                       .BuildServiceProvider()
                                       .GetRequiredService<HttpClientFactory>()
            };

            yield return new HttpClientFactory[]
            {
                new ServiceCollection().AddOptions()
                                       .AddRestack().AddPolly()
                                       .AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent"))
                                       .AddRestackGlobalPolicy(b => Policy.TimeoutAsync<HttpResponseMessage>(3))
                                       .BuildServiceProvider()
                                       .GetRequiredService<HttpClientFactory>()
            };
        }
    }
}

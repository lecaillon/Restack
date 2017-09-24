using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Serilog;
using Xunit;

namespace Restack.Tests
{
    public class RestackTest
    {
        [Fact(DisplayName = "HttpClientFactory_works")]
        public void HttpClientFactory_works()
        {
            // Arrange
            var serviceProvider = TestUtil.ServiceCollection.BuildServiceProvider();
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

        [Fact(DisplayName = "Logging_works")]
        public async void Logging_works()
        {
            // Arrange
            var messages = new StringWriter();
            Log.Logger = new LoggerConfiguration().WriteTo.TextWriter(messages).CreateLogger();

            var serviceProvider = TestUtil.ServiceCollection.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddSerilog();

            var clientFactory = serviceProvider.GetRequiredService<HttpClientFactory>();
            var httpClient = clientFactory.GetDefaultClient();

            // Act
            var response = await httpClient.GetAsync("https://google.com/");

            // Assert
            messages.ToString().Should().NotBeEmpty();
        }

        private static IEnumerable<HttpClientFactory[]> InitHttpClientFactory()
        {
            yield return new HttpClientFactory[]
            {
                TestUtil.ServiceCollection.AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent"))
                                          .BuildServiceProvider()
                                          .GetRequiredService<HttpClientFactory>()
            };

            yield return new HttpClientFactory[]
            {
                TestUtil.ServiceCollection.AddPolly()
                                          .AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent"))
                                          .AddRestackGlobalPolicy(b => Policy.TimeoutAsync<HttpResponseMessage>(3))
                                          .BuildServiceProvider()
                                          .GetRequiredService<HttpClientFactory>()
            };
        }
    }
}

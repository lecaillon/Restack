using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Restack.Refit;
using Restack.Tests.Services;
using Xunit;

namespace Restack.Tests
{
    public class ConsulTest
    {
        [Fact(DisplayName = "Consul_works")]
        public async void Consul_works()
        {
            // Arrange
            var serviceProvider = TestUtil.ServiceCollection
                                          .AddRestClient<IGeoApi>("http://GeoApi")
                                          .BuildServiceProvider();

            var geoApi = serviceProvider.GetRequiredService<IRestClient<IGeoApi>>()
                                        .Client;

            var clientFactory = serviceProvider.GetRequiredService<HttpClientFactory>();
            var httpClient = clientFactory.GetDefaultClient();

            // Act
            var response = await httpClient.GetAsync("https://google.com/");
            var regions = await geoApi.GetRegionsAsync();

            // Assert
            regions.Should().NotBeEmpty();
            regions.First().Code.Should().NotBeNullOrEmpty();
            regions.First().Nom.Should().NotBeNullOrEmpty();

            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}

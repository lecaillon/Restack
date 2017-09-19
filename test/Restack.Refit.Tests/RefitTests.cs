using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Restack.Refit.Tests.Services;
using Xunit;

namespace Restack.Refit.Tests
{
    public class RefitTest
    {
        [Fact(DisplayName = "Refit_works")]
        public void Refit_works()
        {
            // Arrange
            var serviceProvider = InitIGeoApi().BuildServiceProvider();
            var geoApi = serviceProvider.GetRequiredService<IRestClient<IGeoApi>>().Client;

            // Act
            var regions = geoApi.GetRegionsAsync().Result;

            // Assert
            regions.Should().NotBeEmpty();
            regions.First().Code.Should().NotBeNullOrEmpty();
            regions.First().Nom.Should().NotBeNullOrEmpty();
        }

        [Fact(DisplayName = "Refit_with_multi_header_values_works")]
        public void Refit_with_multi_header_values_works()
        {
            // Arrange
            var serviceProvider = InitIGeoApi().AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent"))
                                               .AddRestackHeaders<IGeoApi>(o => o.Headers.Add("key1", "value1"))
                                               .AddRestackHeaders("IGeoApi", o => o.Headers.Add("api-key", "xxxxx"))
                                               .BuildServiceProvider();

            var geoApi = serviceProvider.GetRequiredService<IRestClient<IGeoApi>>().Client;

            // Act
            var response = geoApi.GetRegionsHttpResponseAsync().Result;

            // Assert
            response.RequestMessage.Headers.UserAgent.ToString().Should().Be("myagent");
            response.RequestMessage.Headers.GetValues("key1").First().ToString().Should().Be("value1");
            response.RequestMessage.Headers.GetValues("api-key").First().ToString().Should().Be("xxxxx");
        }

        private IServiceCollection InitIGeoApi() => new ServiceCollection().AddOptions()
                                                                           .AddRestack()
                                                                           .AddRestClient<IGeoApi>("https://geo.api.gouv.fr");
    }
}

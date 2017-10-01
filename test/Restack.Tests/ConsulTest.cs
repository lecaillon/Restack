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
        public void Consul_works()
        {
            // Arrange
            var geoApi = TestUtil.ServiceCollection.AddConsul()
                                                   .AddRestClient<IGeoApi>("http://GeoApi")
                                                   .BuildServiceProvider()
                                                   .GetRequiredService<IRestClient<IGeoApi>>()
                                                   .Client;

            // Act
            var regions = geoApi.GetRegionsAsync().Result;

            // Assert
            regions.Should().NotBeEmpty();
            regions.First().Code.Should().NotBeNullOrEmpty();
            regions.First().Nom.Should().NotBeNullOrEmpty();
        }
    }
}

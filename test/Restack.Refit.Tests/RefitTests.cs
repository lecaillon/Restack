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
            var serviceProvider = new ServiceCollection().AddOptions()
                                                         .AddRestack()
                                                         .AddRestClient<IGeoApi>("https://geo.api.gouv.fr")
                                                         .BuildServiceProvider();

            var geoClient = serviceProvider.GetRequiredService<IRestClient<IGeoApi>>();
            geoClient.Should().NotBeNull();

            var httpClient = geoClient.Client;
            httpClient.Should().NotBeNull();

            var regions = httpClient.GetRegionsAsync().Result;
            regions.Should().NotBeEmpty();
            regions.First().Code.Should().NotBeNullOrEmpty();
            regions.First().Nom.Should().NotBeNullOrEmpty();
        }
    }
}

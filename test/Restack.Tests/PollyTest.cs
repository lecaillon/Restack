using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Timeout;
using Restack.Refit;
using Restack.Tests.Services;
using Xunit;

namespace Restack.Tests
{
    public class PollyTest
    {
        [Fact(DisplayName = "Polly_works")]
        public void Polly_works()
        {
            // Arrange
            int nbTry = 0;
            var geoApi = TestUtil.ServiceCollection.AddRestClient<IGeoApi>("https://bad.url.geo.api.gouv.fr")
                                                   .AddRestackGlobalPolicy(b => Policy.TimeoutAsync<HttpResponseMessage>(3)) // Polly
                                                   .AddRestackPolicy<IGeoApi>(b => b.RetryForeverAsync((res, ctx) => nbTry++)) // Polly
                                                   .BuildServiceProvider()
                                                   .GetRequiredService<IRestClient<IGeoApi>>()
                                                   .Client;

            // Acts
            Func<Task<IEnumerable<Region>>> func = async () => await geoApi.GetRegionsAsync();

            // Assert
            func.ShouldThrowExactly<AggregateException>()
                .WithInnerExceptionExactly<TimeoutRejectedException>();

            nbTry.Should().BeGreaterThan(0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
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
            var geoApi = new ServiceCollection().AddOptions()
                                                .AddRestack().AddPolly()
                                                .AddRestClient<IGeoApi>("https://bad.url.geo.api.gouv.fr")
                                                .AddRestackGlobalPolicy(b => Policy.TimeoutAsync<HttpResponseMessage>(3)) // Polly
                                                .AddRestackPolicy<IGeoApi>(b => b.RetryForeverAsync((res, ctx) => nbTry++)) // Polly
                                                .BuildServiceProvider()
                                                .GetRequiredService<IRestClient<IGeoApi>>()
                                                .Client;

            // Acts
            Func<IEnumerable<Region>> func = () => geoApi.GetRegionsAsync().Result;

            // Assert
            func.Enumerating().ShouldThrowExactly<AggregateException>()
                              .WithInnerExceptionExactly<TimeoutRejectedException>();

            nbTry.Should().BeGreaterThan(0);
        }
    }
}

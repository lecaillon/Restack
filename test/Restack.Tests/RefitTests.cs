using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Restack.Refit;
using Restack.Tests.Services;
using Xunit;

namespace Restack.Tests
{
    public class RefitTest
    {
        [Theory(DisplayName = "Refit_works")]
        [MemberData(nameof(InitIGeoApi))]
        public void Refit_works(IGeoApi geoApi)
        {
            // Act
            var regions = geoApi.GetRegionsAsync().Result;

            // Assert
            regions.Should().NotBeEmpty();
            regions.First().Code.Should().NotBeNullOrEmpty();
            regions.First().Nom.Should().NotBeNullOrEmpty();
        }

        [Theory(DisplayName = "Refit_with_multi_header_values_works")]
        [MemberData(nameof(InitIGeoApiWithHeaders))]
        public void Refit_with_multi_header_values_works(IGeoApi geoApi)
        {
            // Act
            var response = geoApi.GetRegionsHttpResponseAsync().Result;

            // Assert
            response.RequestMessage.Headers.UserAgent.ToString().Should().Be("myagent");
            response.RequestMessage.Headers.GetValues("key1").First().ToString().Should().Be("value1");
            response.RequestMessage.Headers.GetValues("api-key").First().ToString().Should().Be("xxxxx");
        }

        private static IEnumerable<IGeoApi[]> InitIGeoApi()
        {
            yield return new IGeoApi[] 
            {
                new ServiceCollection().AddOptions()
                                       .AddRestack()
                                       .AddRestClient<IGeoApi>("https://geo.api.gouv.fr") // refit
                                       .BuildServiceProvider()
                                       .GetRequiredService<IRestClient<IGeoApi>>()
                                       .Client
        };

            yield return new IGeoApi[]
            {
                new ServiceCollection().AddOptions()
                                       .AddRestack().AddPolly()
                                       .AddRestClient<IGeoApi>("https://geo.api.gouv.fr") // refit
                                       .AddRestackPolicy<IGeoApi>(b => b.RetryAsync()) // Polly
                                       .AddRestackPolicy<IGeoApi>(b => b.CircuitBreakerAsync(1, TimeSpan.FromSeconds(5))) // Polly
                                       .BuildServiceProvider()
                                       .GetRequiredService<IRestClient<IGeoApi>>()
                                       .Client
            };
        }

        private static IEnumerable<IGeoApi[]> InitIGeoApiWithHeaders()
        {
            yield return new IGeoApi[]
            {
                new ServiceCollection().AddOptions()
                                       .AddRestack()
                                       .AddRestClient<IGeoApi>("https://geo.api.gouv.fr") // refit
                                       .AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent")) // header
                                       .AddRestackHeaders<IGeoApi>(o => o.Headers.Add("key1", "value1")) // header
                                       .AddRestackHeaders("IGeoApi", o => o.Headers.Add("api-key", "xxxxx")) // header
                                       .BuildServiceProvider()
                                       .GetRequiredService<IRestClient<IGeoApi>>()
                                       .Client
            };

            yield return new IGeoApi[]
            {
                new ServiceCollection().AddOptions()
                                       .AddRestack().AddPolly()
                                       .AddRestClient<IGeoApi>("https://geo.api.gouv.fr") // refit
                                       .AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent")) // header
                                       .AddRestackHeaders<IGeoApi>(o => o.Headers.Add("key1", "value1")) // header
                                       .AddRestackHeaders("IGeoApi", o => o.Headers.Add("api-key", "xxxxx")) // header
                                       .AddRestackPolicy<IGeoApi>(b => b.RetryAsync()) // Polly
                                       .AddRestackPolicy<IGeoApi>(b => b.CircuitBreakerAsync(1, TimeSpan.FromSeconds(5))) // Polly
                                       .BuildServiceProvider()
                                       .GetRequiredService<IRestClient<IGeoApi>>()
                                       .Client
            };
        }
    }
}

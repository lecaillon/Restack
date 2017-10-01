using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Options;
using Preconditions;

namespace Restack.Consul
{
    public class ConsulServiceDiscoveryClient : IServiceDiscoveryClient
    {
        private readonly ConsulClient _client;

        public ConsulServiceDiscoveryClient(IOptions<ConsulOptions> consulOptions)
        {
            Check.NotNull(consulOptions, nameof(consulOptions));

            _client = consulOptions.Value.ConsulClient ?? throw new ArgumentNullException("ConsulClient");
        }

        public async Task<Uri> GetUriAsync(Uri service)
        {
            return await GetUriAsync(service, default(CancellationToken));
        }

        public async Task<Uri> GetUriAsync(Uri service, CancellationToken token)
        {
            var result = await _client.Catalog.Service(service.Host, token);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ConsulRequestException("Non-successs status code from Consul during service discovery.", result.StatusCode);
            }

            if (result.Response.Any())
            {
                var serviceToRouteTo = PickRandom(result.Response); // cheap load balancing policy when not using DNS
                var builder = new UriBuilder(serviceToRouteTo.ServiceAddress)
                {
                    Port = serviceToRouteTo.ServicePort,
                    Path = service.AbsolutePath,
                    Query = service.Query,
                };

                return new Uri(builder.ToString());
            }
            else
            {
                return service;
            }
        }

        private static T PickRandom<T>(IEnumerable<T> source) => source.OrderBy(x => Guid.NewGuid()).Take(1).First();
    }
}

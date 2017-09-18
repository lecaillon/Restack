using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restack;
using Restack.Refit;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestClient(this IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IRestClient<>), typeof(RestClient<>));
            return services;
        }

        public static IServiceCollection AddRestClient<TClient>(this IServiceCollection services, Action<HttpClientOptions> action)
        {
            services.AddRestClient();
            services.Configure(typeof(TClient).Name, action);
            return services;
        }

        public static IServiceCollection AddRestClient<TClient>(this IServiceCollection services, string uri)
        {
            services.AddRestClient<TClient>(o => o.BaseAddress = new Uri(uri));
            return services;
        }
    }
}

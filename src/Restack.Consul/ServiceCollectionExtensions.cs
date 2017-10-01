using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Preconditions;
using Restack.Consul;
using Restack.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, ConsulHttpClientFactoryPolicy>());
            services.TryAddSingleton<IServiceDiscoveryClient, ConsulServiceDiscoveryClient>();

            return services;
        }

        public static IServiceCollection AddConsul(this IServiceCollection services, Action<ConsulOptions> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(setupAction, nameof(setupAction));

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, ConsulHttpClientFactoryPolicy>());
            services.TryAddSingleton<IServiceDiscoveryClient, ConsulServiceDiscoveryClient>();
            services.Configure(setupAction);

            return services;
        }
    }
}

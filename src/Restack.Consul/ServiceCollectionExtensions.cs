using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Preconditions;
using Restack.Http;

namespace Restack.Consul
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

        public static IServiceCollection AddRestackServiceDiscovery<TClient>(this IServiceCollection services, Action<ConsulOptions> action)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(action, nameof(action));

            services.Configure<ConsulOptions>(typeof(TClient).Name, action);
            return services;
        }

        public static IServiceCollection AddRestackServiceDiscovery(this IServiceCollection services, string name, Action<ConsulOptions> action)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(name, nameof(name));
            Check.NotNull(action, nameof(action));

            services.Configure<ConsulOptions>(name, action);
            return services;
        }
    }
}

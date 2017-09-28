using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Preconditions;
using Restack.Http;

namespace Restack.Consul
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, Action<ConsulOptions> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(setupAction, nameof(setupAction));

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, ConsulHttpClientFactoryPolicy>());
            services.TryAddSingleton<IServiceDiscoveryClient, ConsulServiceDiscoveryClient>();
            services.Configure(setupAction);

            return services;
        }

        public static IServiceCollection AddRestackServiceDiscovery<TClient>(this IServiceCollection services, Action<ServiceDiscoveryOptions> configureService)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(configureService, nameof(configureService));

            return AddRestackServiceDiscovery(services, typeof(TClient).Name, configureService);
        }

        public static IServiceCollection AddRestackServiceDiscovery(this IServiceCollection services, string name, Action<ServiceDiscoveryOptions> configureService)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(name, nameof(name));
            Check.NotNull(configureService, nameof(configureService));

            services.Configure<ServiceDiscoveryOptions>(name, configureService);
            return services;
        }
    }
}

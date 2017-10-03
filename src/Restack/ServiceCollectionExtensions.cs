using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Preconditions;
using Restack;
using Restack.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestack(this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            services.TryAddSingleton<HttpClientFactory, DefaultHttpClientFactory>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, OptionsHttpClientFactoryPolicy>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, HeaderHttpClientFactoryPolicy>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, LoggingHttpClientFactoryPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, LoggingScopeHttpClientFactoryPolicy>());

            return services;
        }

        public static IServiceCollection AddRestackGlobalHeaders(this IServiceCollection services, Action<HeaderOptions> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(setupAction, nameof(setupAction));

            services.ConfigureAll<HeaderOptions>(setupAction);
            return services;
        }

        public static IServiceCollection AddRestackHeaders(this IServiceCollection services, string name, Action<HeaderOptions> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(name, nameof(name));
            Check.NotNull(setupAction, nameof(setupAction));

            services.Configure<HeaderOptions>(name, setupAction);
            return services;
        }

        public static IServiceCollection AddRestackHeaders<TClient>(this IServiceCollection services, Action<HeaderOptions> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(setupAction, nameof(setupAction));

            services.Configure<HeaderOptions>(typeof(TClient).Name, setupAction);
            return services;
        }

        public static IServiceCollection AddRestackNamedClient(this IServiceCollection services, string name, Action<HttpClientOptions> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(name, nameof(name));
            Check.NotNull(setupAction, nameof(setupAction));

            services.Configure<HttpClientOptions>(name, setupAction);
            return services;
        }
    }
}

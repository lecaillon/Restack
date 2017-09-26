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

        public static IServiceCollection AddRestackGlobalHeaders(this IServiceCollection services, Action<HeaderOptions> action)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(action, nameof(action));

            services.ConfigureAll<HeaderOptions>(action);
            return services;
        }

        public static IServiceCollection AddRestackHeaders(this IServiceCollection services, string name, Action<HeaderOptions> action)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(name, nameof(name));
            Check.NotNull(action, nameof(action));

            services.Configure<HeaderOptions>(name, action);
            return services;
        }

        public static IServiceCollection AddRestackHeaders<TClient>(this IServiceCollection services, Action<HeaderOptions> action)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(action, nameof(action));

            services.Configure<HeaderOptions>(typeof(TClient).Name, action);
            return services;
        }

        public static IServiceCollection AddRestackNamedClient(this IServiceCollection services, string name, Action<HttpClientOptions> action)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(name, nameof(name));
            Check.NotNull(action, nameof(action));

            services.Configure<HttpClientOptions>(name, action);
            return services;
        }
    }
}

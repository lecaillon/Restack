using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Preconditions;
using Restack;
using Restack.Internal;

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
            
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, LoggingScopeHttpClientFactoryPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, MessageLoggingHttpClientFactoryPolicy>());

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
    }
}

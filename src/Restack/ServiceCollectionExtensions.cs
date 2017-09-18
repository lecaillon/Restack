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

            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, DefaultHeaderHttpClientFactoryPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, LoggingScopeHttpClientFactoryPolicy>());
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, MessageLoggingHttpClientFactoryPolicy>());

            return services;
        }
    }
}

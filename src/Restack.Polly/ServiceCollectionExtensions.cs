using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Preconditions;
using Restack.Http;
using Restack.Polly;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPolly(this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, PollyHttpClientFactoryPolicy>());

            return services;
        }

        public static IServiceCollection AddRestackGlobalPolicy(this IServiceCollection services, Func<PolicyBuilder<HttpResponseMessage>, Policy<HttpResponseMessage>> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(setupAction, nameof(setupAction));

            var builder = Policy<HttpResponseMessage>.Handle<HttpRequestException>().OrResult(m => !m.IsSuccessStatusCode);
            services.ConfigureAll<PollyOptions>(options => options.Policies.Add(setupAction(builder)));

            return services;
        }

        public static IServiceCollection AddRestackPolicy(this IServiceCollection services, string name, Func<PolicyBuilder<HttpResponseMessage>, Policy<HttpResponseMessage>> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(name, nameof(name));
            Check.NotNull(setupAction, nameof(setupAction));

            var builder = Policy<HttpResponseMessage>.Handle<HttpRequestException>().OrResult(m => !m.IsSuccessStatusCode);
            services.Configure<PollyOptions>(name, options => options.Policies.Add(setupAction(builder)));
            return services;
        }

        public static IServiceCollection AddRestackPolicy<TClient>(this IServiceCollection services, Func<PolicyBuilder<HttpResponseMessage>, Policy<HttpResponseMessage>> setupAction)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNull(setupAction, nameof(setupAction));

            return AddRestackPolicy(services, typeof(TClient).Name, setupAction);
        }
    }
}

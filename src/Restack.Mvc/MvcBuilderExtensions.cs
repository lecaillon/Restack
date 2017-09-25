using Microsoft.AspNetCore.Mvc;
using Preconditions;
using Restack.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcBuilderExtensions
    {
        public static IMvcCoreBuilder AddRestackModelBinder(this IMvcCoreBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, new HttpClientModelBinderProvider());
            });

            return builder;
        }

        public static IMvcBuilder AddRestackModelBinder(this IMvcBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services.Configure<MvcOptions>(options =>
            {
                options.ModelBinderProviders.Insert(0, new HttpClientModelBinderProvider());
            });

            return builder;
        }
    }
}

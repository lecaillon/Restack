using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Preconditions;

namespace Restack.Mvc
{
    internal class HttpClientModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            Check.NotNull(context, nameof(context));

            if (context.Metadata.UnderlyingOrModelType == typeof(HttpClient))
            {
                var clientName = context.BindingInfo.BindingSource is HttpClientBindingSource source ? source.ClientName : HttpClientFactory.DefaultName;
                return new HttpClientModelBinder(clientName);
            }

            return null;
        }
    }
}

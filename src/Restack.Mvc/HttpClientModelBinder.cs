using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Preconditions;

namespace Restack.Mvc
{
    internal class HttpClientModelBinder : IModelBinder
    {
        public HttpClientModelBinder(string clientName)
        {
            ClientName = Check.NotNull(clientName, nameof(clientName));
        }

        public string ClientName { get; }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Check.NotNull(bindingContext, nameof(bindingContext));

            var clientFactory = bindingContext.HttpContext.RequestServices.GetRequiredService<HttpClientFactory>();
            var client = clientFactory.GetClient(ClientName);

            bindingContext.Result = ModelBindingResult.Success(client);
            bindingContext.ValidationState.Add(client, new ValidationStateEntry()
            {
                SuppressValidation = true,
            });

            return Task.CompletedTask;
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Preconditions;

namespace Restack.Mvc
{
    public class HttpClientBindingSource : BindingSource
    {
        public HttpClientBindingSource(string clientName) : base(Special.Id, Special.DisplayName, isGreedy: true, isFromRequest: false)
        {
            ClientName = Check.NotNull(clientName, nameof(clientName));
        }

        public string ClientName { get; }
    }
}

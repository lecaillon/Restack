using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Preconditions;

namespace Restack.Mvc
{
    public class HttpClientNameAttribute : Attribute, IBindingSourceMetadata
    {
        public HttpClientNameAttribute(string clientName)
        {
            ClientName = Check.NotNull(clientName, nameof(clientName));
            BindingSource = new HttpClientBindingSource(ClientName);
        }

        public BindingSource BindingSource { get; }

        public string ClientName { get; }
    }
}

using System;
using System.Net.Http;

namespace Restack.Http
{
    public abstract class HttpClientFactoryContext
    {
        public abstract Uri BaseAddress { get; set; }

        public abstract string Name { get; }

        public abstract HttpMessageHandler MessageHandler { get; set; }

        public abstract void PrependMessageHandler(DelegatingHandler messageHandler);
    }
}

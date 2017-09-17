using System;
using System.Net.Http;

namespace Restack
{
    public class HttpClientOptions
    {
        public Uri BaseAddress { get; set; }
        public HttpMessageHandler MessageHandler { get; set; }
    }
}

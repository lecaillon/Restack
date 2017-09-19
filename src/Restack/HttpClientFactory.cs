using System.Net.Http;

namespace Restack
{
    public abstract class HttpClientFactory
    {
        public static readonly string DefaultName = "Default";

        public virtual HttpClient GetDefaultClient()
        {
            return GetClient(DefaultName);
        }

        public abstract HttpClient GetClient(string name);
    }
}

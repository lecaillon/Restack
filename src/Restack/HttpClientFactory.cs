using System.Net.Http;

namespace Restack
{
    public abstract class HttpClientFactory
    {
        public static readonly string DefaultName = "Default";

        public virtual HttpClient GetDefaultClient()
        {
            return GetClientByName(DefaultName);
        }

        public abstract HttpClient GetClientByName(string name);
    }
}

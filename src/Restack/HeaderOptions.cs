using Microsoft.AspNetCore.Http;

namespace Restack
{
    public class HeaderOptions
    {
        public IHeaderDictionary Headers { get; } = new HeaderDictionary();
    }
}

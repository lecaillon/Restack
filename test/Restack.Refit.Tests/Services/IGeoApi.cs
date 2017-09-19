using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace Restack.Refit.Tests.Services
{
    public interface IGeoApi
    {
        [Get("/regions")]
        Task<IEnumerable<Region>> GetRegionsAsync();

        [Get("/regions")]
        Task<HttpResponseMessage> GetRegionsHttpResponseAsync();
    }

    public class Region
    {
        public string Code { get; set; }
        public string Nom { get; set; }
    }
}

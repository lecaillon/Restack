using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Restack.WebApp.Services
{
    public interface IGeoApi
    {
        [Get("/regions")]
        Task<IEnumerable<Region>> GetRegionsAsync();
    }

    public class Region
    {
        public string Code { get; set; }
        public string Nom { get; set; }
    }
}

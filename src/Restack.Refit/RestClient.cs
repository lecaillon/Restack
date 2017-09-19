using Preconditions;
using Refit;

namespace Restack.Refit
{
    public class RestClient<TClient> : IRestClient<TClient>
    {
        private readonly HttpClientFactory _clientFactory;
        private TClient _client;

        public RestClient(HttpClientFactory clientFactory)
        {
            _clientFactory = Check.NotNull(clientFactory, nameof(clientFactory));
        }

        public TClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = RestService.For<TClient>(_clientFactory.GetClient(typeof(TClient).Name));
                }

                return _client;
            }
            set
            {
                _client = value;
            }
        }
    }
}

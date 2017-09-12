using System;
using Refit;

namespace Kickr.Refit
{

    public class RestClient<TClient> : IRestClient<TClient>
    {
        private readonly HttpClientFactory _clientFactory;
        private TClient _client;
        
        public RestClient(HttpClientFactory clientFactory)
        {
            if (clientFactory == null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }

            _clientFactory = clientFactory;
        }

        public TClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = RestService.For<TClient>(_clientFactory.GetNamedClient(typeof(TClient).Name));
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

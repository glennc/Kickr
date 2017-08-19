using System;
using System.Net.Http;

namespace Kickr
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private HttpClient _client;

        public HttpClientFactory(IServiceProvider provider, HttpClientPipelineBuilder builder)
        {
            _client = new HttpClient(builder.Build(provider));
        }

        public HttpClient GetHttpClient()
        {
            return _client;
        }
    }
}

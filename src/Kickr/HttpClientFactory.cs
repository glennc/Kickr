using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Kickr.Options;
using Microsoft.Extensions.Options;

namespace Kickr
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private HttpClient _client;

        public HttpClientFactory(HttpClientPipelineBuilder builder)
        {
            _client = new HttpClient(builder.Build());
        }

        public HttpClient GetHttpClient()
        {
            return _client;
        }
    }
}

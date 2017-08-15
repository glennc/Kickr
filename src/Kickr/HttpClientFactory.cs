using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace Kickr
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private IServiceDiscoveryClient _discoveryClient;
        private HttpClient _client;

        public HttpClientFactory(IServiceDiscoveryClient discoveryClient, IUriPolicyService policyService)
        {
            _discoveryClient = discoveryClient;
            _client = new HttpClient(new PolicyHandler(discoveryClient, policyService));
        }

        public HttpClient GetHttpClient()
        {
            return _client;
        }
    }
}

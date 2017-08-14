using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kickr
{
    public class PolicyHandler : DelegatingHandler
    {
        private const int NUMBER_OF_ALLOWED_ERRORS = 3;
        private IServiceDiscoveryClient _serviceDiscoverer;
        private Dictionary<Uri, Policy<HttpResponseMessage>> _policies;

        public PolicyHandler(IServiceDiscoveryClient serviceDiscoverer)
            :base(new HttpClientHandler())
        {
            _serviceDiscoverer = serviceDiscoverer;
            _policies = new Dictionary<Uri, Policy<HttpResponseMessage>>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = await _serviceDiscoverer.GetUrl(request.RequestUri);

            Policy<HttpResponseMessage> executionPolicy;
            if (!_policies.TryGetValue(request.RequestUri, out executionPolicy))
            {
                executionPolicy = GetPolicy();
                _policies.Add(request.RequestUri, executionPolicy);
            }

            var response = await executionPolicy.ExecuteAsync(t => base.SendAsync(request, t), cancellationToken);

            return response;
        }

        private Policy<HttpResponseMessage> GetPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>((resp) =>
                {
                    return resp.IsSuccessStatusCode;
                })
                .CircuitBreakerAsync(NUMBER_OF_ALLOWED_ERRORS, TimeSpan.FromSeconds(30));
        }
    }
}

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
        private IServiceDiscoveryClient _serviceDiscoverer;
        private IUriPolicyService _policyService;

        //TODO: This should be split into two delegating handlers.
        public PolicyHandler(IServiceDiscoveryClient serviceDiscoverer, IUriPolicyService policyService)
            :base(new HttpClientHandler())
        {
            _serviceDiscoverer = serviceDiscoverer;
            _policyService = policyService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = await _serviceDiscoverer.GetUrl(request.RequestUri);

            var executionPolicy = _policyService.GetPolicy(request.RequestUri);
            var response = await executionPolicy.ExecuteAsync(t => base.SendAsync(request, t), cancellationToken);

            return response;
        }
    }
}

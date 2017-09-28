using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kickr.Consul
{
    internal class ConsulMessageHandler : DelegatingHandler
    {
        private readonly IServiceDiscoveryClient _discoveryClient;

        public ConsulMessageHandler(IServiceDiscoveryClient discoveryClient)
        {
            _discoveryClient = discoveryClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = await _discoveryClient.GetUriAsync(request.RequestUri);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}

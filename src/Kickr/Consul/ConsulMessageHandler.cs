using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kickr.Consul
{
    public class ConsulMessageHandler : DelegatingHandler
    {
        private IServiceDiscoveryClient _client;

        public ConsulMessageHandler(HttpMessageHandler baseHandler, IServiceDiscoveryClient client)
            : base(baseHandler)
        {
            _client = client;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = await _client.GetUrl(request.RequestUri);
            return await base.SendAsync(request, cancellationToken);
        }

    }
}

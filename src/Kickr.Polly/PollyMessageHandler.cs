using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kickr.Polly
{
    internal class PollyMessageHandler : DelegatingHandler
    {
        private readonly IOptionsMonitor<PollyOptions> _options;
        private readonly string _clientName;

        public PollyMessageHandler(IOptionsMonitor<PollyOptions> optionsFactory, string clientName)
        {
            _options = optionsFactory;
            _clientName = clientName;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = GetOptions(request);
            if (options == null || options.Policies.Count == 0)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            return await options.Policies.Combine().ExecuteAsync(t => base.SendAsync(request, t), cancellationToken);
        }

        private PollyOptions GetOptions(HttpRequestMessage request)
        {
            if (_clientName != HttpClientFactory.DefaultClientName)
            {
                return _options.Get(_clientName);
            }

            if (request.RequestUri.HostNameType == UriHostNameType.Dns)
            {
                return _options.Get(request.RequestUri.Host);
            }

            return null;
        }
    }
}

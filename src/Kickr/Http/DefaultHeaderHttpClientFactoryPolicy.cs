using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kickr.Http
{
    internal class DefaultHeaderHttpClientFactoryPolicy : IHttpClientFactoryPolicy
    {
        private readonly IOptionsMonitor<HeaderOptions> _options;

        public DefaultHeaderHttpClientFactoryPolicy(IOptionsMonitor<HeaderOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public int Order => 1000;

        public void Apply(HttpClientFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.PrependMessageHandler(new DefaultHeaderMessageHandler(_options, context.ClientName));
        }

        private class DefaultHeaderMessageHandler : DelegatingHandler
        {
            private readonly IOptionsMonitor<HeaderOptions> _options;
            private readonly string _clientName;

            public DefaultHeaderMessageHandler(IOptionsMonitor<HeaderOptions> options, string clientName)
            {
                _options = options;
                _clientName = clientName;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var options = GetOptions(request);
                foreach (var kvp in options.Headers)
                {
                    if (!request.Headers.TryAddWithoutValidation(kvp.Key, (IEnumerable<string>)kvp.Value))
                    {
                        request.Content?.Headers.TryAddWithoutValidation(kvp.Key, (IEnumerable<string>)kvp.Value);
                    }
                }

                return base.SendAsync(request, cancellationToken);
            }

            private HeaderOptions GetOptions(HttpRequestMessage request)
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
}

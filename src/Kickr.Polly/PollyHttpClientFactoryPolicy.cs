using System;
using System.Net.Http;
using Kickr.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kickr.Polly
{
    internal class PollyHttpClientFactoryPolicy : AbstractDelegatingHandlerHttpClientFactoryPolicy
    {
        private readonly IOptionsMonitor<PollyOptions> _options;

        public PollyHttpClientFactoryPolicy(IOptionsMonitor<PollyOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public override int Order => 0;

        protected override DelegatingHandler CreateHandler(HttpClientFactoryContext context)
        {
            return new PollyMessageHandler(_options, context.ClientName);
        }
    }
}


using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kickr.Http
{
    internal class NamedClientBaseAddessHttpClientFactoryPolicy : IHttpClientFactoryPolicy
    {
        private readonly IOptionsMonitor<NamedClientOptions> _options;

        public NamedClientBaseAddessHttpClientFactoryPolicy(IOptionsMonitor<NamedClientOptions> options)
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

            if (context.ClientName == HttpClientFactory.DefaultClientName)
            {
                return;
            }

            var options = _options.Get(context.ClientName);
            context.BaseAddress = options.BaseAddress;
        }
    }
}

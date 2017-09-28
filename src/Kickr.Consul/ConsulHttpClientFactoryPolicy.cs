using System;
using System.Net.Http;
using Kickr.Http;

namespace Kickr.Consul
{
    internal class ConsulHttpClientFactoryPolicy : AbstractDelegatingHandlerHttpClientFactoryPolicy
    {
        private readonly IServiceDiscoveryClient _discoveryClient;

        public ConsulHttpClientFactoryPolicy(IServiceDiscoveryClient discoveryClient)
        {
            if (discoveryClient == null)
            {
                throw new ArgumentNullException(nameof(discoveryClient));
            }

            _discoveryClient = discoveryClient;
        }

        public override int Order => 2000;

        protected override DelegatingHandler CreateHandler(HttpClientFactoryContext context)
        {
            return new ConsulMessageHandler(_discoveryClient);
        }
    }
}

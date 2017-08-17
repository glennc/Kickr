using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Kickr
{
    public class HeaderHandler : DelegatingHandler
    {
        private IOptionsMonitor<HeaderOptions> _optionsFactory;
        private UriKeyGenerator _keyGenerator;

        public HeaderHandler(IOptionsMonitor<HeaderOptions> optionsFactory, UriKeyGenerator keyGenerator)
        {
            _optionsFactory = optionsFactory;
            _keyGenerator = keyGenerator;
        }

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var uriOptions = _optionsFactory.Get(_keyGenerator(request.RequestUri));



            return await base.SendAsync(request, cancellationToken);
        }
	}
}

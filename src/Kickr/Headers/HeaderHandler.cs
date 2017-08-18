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
        private IUriKeyGenerator _keyGenerator;

        public HeaderHandler(HttpMessageHandler baseHandler, IOptionsMonitor<HeaderOptions> optionsFactory, IUriKeyGenerator keyGenerator)
            : base(baseHandler)
        {
            _optionsFactory = optionsFactory;
            _keyGenerator = keyGenerator;
        }

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = _optionsFactory.Get(_keyGenerator.GenerateKey(request.RequestUri));

            foreach(var header in options.Headers)
            {
                request.Headers.Add(header.Key, header.Value.ToArray());
            }

            return await base.SendAsync(request, cancellationToken);
        }
	}
}

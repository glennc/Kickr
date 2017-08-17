using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Kickr.Headers
{
    public class HeaderHandler : DelegatingHandler
    {
        private IOptionsFactory<HeaderOptions> _optionsFactory;
        private UriKeyGenerator _keyGenerator;

        public HeaderHandler(IOptionsFactory<HeaderOptions> optionsFactory, UriKeyGenerator keyGenerator)
        {
            _optionsFactory = optionsFactory;
            _keyGenerator = keyGenerator;
        }

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var common = _optionsFactory.Create("Common");
            var defOptions = _optionsFactory.Create("Default");
            return await base.SendAsync(request, cancellationToken);
        }
	}
}

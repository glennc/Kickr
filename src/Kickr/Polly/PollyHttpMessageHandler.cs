using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kickr.Policy
{
    public class PollyHttpMessageHandler : DelegatingHandler
    {
        private IOptionsMonitor<PollyUriOptions> _optionsFactory;
        private IUriKeyGenerator _keyGenerator;

        public PollyHttpMessageHandler(IOptionsMonitor<PollyUriOptions> optionsFactory, HttpMessageHandler baseHandler, IUriKeyGenerator keyGenerator)
            : base(baseHandler)
        {
            _optionsFactory = optionsFactory;
            _keyGenerator = keyGenerator;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = _optionsFactory.Get(_keyGenerator.GenerateKey(request.RequestUri));

            if (options.Policy == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            return await options.Policy.ExecuteAsync(t => base.SendAsync(request, t), cancellationToken);
        }
    }
}

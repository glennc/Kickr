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
        private IOptionsFactory<PollyUriOptions> _optionsFactory;
        private UriKeyGenerator _keyGenerator;

        public PollyHttpMessageHandler(IOptionsFactory<PollyUriOptions> optionsFactory, HttpMessageHandler baseHandler, UriKeyGenerator keyGenerator)
            : base(baseHandler)
        {
            _optionsFactory = optionsFactory;
            _keyGenerator = keyGenerator;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = _optionsFactory.Create(_keyGenerator(request.RequestUri));

            if (options.Policy == null)
            {
                var defaultOptions = _optionsFactory.Create("Default");
                if (defaultOptions.Policy == null)
                {
                    return await base.SendAsync(request, cancellationToken);
                }
                options = defaultOptions;
            }

            return await options.Policy.ExecuteAsync(t => base.SendAsync(request, t), cancellationToken);
        }
    }
}

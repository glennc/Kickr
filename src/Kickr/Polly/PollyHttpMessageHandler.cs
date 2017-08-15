using Kickr.Options;
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

        public PollyHttpMessageHandler(IOptionsFactory<PollyUriOptions> optionsFactory, HttpMessageHandler baseHandler)
            : base(baseHandler)
        {
            _optionsFactory = optionsFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = _optionsFactory.Create($"{request.RequestUri.Host}:{request.RequestUri.Port}");

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

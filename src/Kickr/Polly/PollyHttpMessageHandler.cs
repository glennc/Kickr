using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Kickr.Policy
{
    public class PollyHttpMessageHandler : DelegatingHandler
    {
        private IOptionsMonitor<PollyUriOptions> _optionsFactory;
        private IUriKeyGenerator _keyGenerator;

        public PollyHttpMessageHandler(IOptionsMonitor<PollyUriOptions> optionsFactory, IUriKeyGenerator keyGenerator)
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

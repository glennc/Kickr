using Kickr.Options;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kickr
{
    //public class PolicyHandler : DelegatingHandler
    //{
    //    private IServiceDiscoveryClient _serviceDiscoverer;
    //    private IOptionsFactory<PollyUriOptions> _options;

    //    //TODO: This should be split into two delegating handlers.
    //    public PolicyHandler(IServiceDiscoveryClient serviceDiscoverer, IOptionsFactory<PollyUriOptions> options)
    //        :base(new HttpClientHandler())
    //    {
    //        _serviceDiscoverer = serviceDiscoverer;
    //        _options = options;
    //    }

    //    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        request.RequestUri = await _serviceDiscoverer.GetUrl(request.RequestUri);

    //        var options = _options.Create($"{request.RequestUri.Host}:{request.RequestUri.Port}");

    //        if (options.Policy == null)
    //        {
    //            var defaultOptions = _options.Create("Default");
    //            if (defaultOptions.Policy == null)
    //            {
    //                return await base.SendAsync(request, cancellationToken);
    //            }
    //            options = defaultOptions;
    //        }

    //        return await options.Policy.ExecuteAsync(t => base.SendAsync(request, t), cancellationToken);
    //    }
    //}
}

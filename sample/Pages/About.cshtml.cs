using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Kickr;
using System.Net.Http;
using Polly.CircuitBreaker;
using Microsoft.Extensions.Logging;
using System.Threading;
using Kickr.Consul;
using Consul;

namespace sample.Pages
{
    public class AboutModel : PageModel
    {
        private ILogger<AboutModel> _logger;
        private IConsulClient _cilent;

        public string Message { get; set; }

        public AboutModel(ILogger<AboutModel> logger, IConsulClient client)
        {
            _logger = logger;
            _cilent = client;
        }

        public async Task OnGet()
        {
            Message = "Your application description page.";

            IServiceDiscoveryClient services = new ConsulServiceDiscoveryClient(_cilent);
            var client = new HttpClient(new PolicyHandler(services));
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var s = await client.GetStringAsync("http://sample");
                }
                catch (BrokenCircuitException ex)
                {
                    //Circuit has been tripped. Send some default data or otherwise handle the fact that your thing is broken.
                    _logger.LogError(ex, "Circuit broken");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "error calling service");
                }
            }
        }

    }

    public class DummyServiceDiscovery : IServiceDiscoveryClient
    {
        public Task<Uri> GetUrl(Uri service)
        {
            return GetUrl(service, default(CancellationToken));
        }

        public Task<Uri> GetUrl(Uri service, CancellationToken token)
        {
            return Task.FromResult(service);
        }
    }
}

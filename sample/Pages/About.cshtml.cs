using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Kickr;
using System.Net.Http;
using Polly.CircuitBreaker;
using Microsoft.Extensions.Logging;

namespace sample.Pages
{
    public class AboutModel : PageModel
    {
        private ILogger<AboutModel> _logger;

        public string Message { get; set; }

        public AboutModel(ILogger<AboutModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            Message = "Your application description page.";

            IServiceDiscoveryClient services = new DummyServiceDiscovery();
            var client = new HttpClient(new PolicyHandler(services));
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var s = await client.GetStringAsync("http://localhost:8080");
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
        public Uri GetUrl(Uri service)
        {
            return service;
        }
    }
}

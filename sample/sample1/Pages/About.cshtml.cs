using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kickr;
using Kickr.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace sample.Pages
{
    public class AboutModel : PageModel
    {
        private ILogger<AboutModel> _logger;
        private HttpClientFactory _cilentFactory;

        public string Message { get; set; }

        public AboutModel(ILogger<AboutModel> logger, HttpClientFactory clientFactory)
        {
            _logger = logger;
            _cilentFactory = clientFactory;
        }

        public async Task OnGet([HttpClientName("github")] HttpClient client)
        {
            Message = await client.GetStringAsync("/");

            //for (int i = 0; i < 10; i++)
            //{
            //    try
            //    {
            //        var s = await client.GetStringAsync("http://sample");
            //    }
            //    catch (BrokenCircuitException ex)
            //    {
            //        //Circuit has been tripped. Send some default data or otherwise handle the fact that your thing is broken.
            //        _logger.LogError(ex, "Circuit broken");
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "error calling service");
            //    }
            //}

        }

    }

    public class DummyServiceDiscovery : IServiceDiscoveryClient
    {
        public Task<Uri> GetUriAsync(Uri service)
        {
            return GetUriAsync(service, default(CancellationToken));
        }

        public Task<Uri> GetUriAsync(Uri service, CancellationToken token)
        {
            return Task.FromResult(service);
        }
    }
}

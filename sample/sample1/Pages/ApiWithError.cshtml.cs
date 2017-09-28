using System;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Kickr.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;

namespace sample.Pages
{
    public class ApiWithErrorModel : PageModel
    {
        private ILogger<ApiWithErrorModel> _logger;
        private HtmlEncoder _encoder;

        public IHtmlContent Payload { get; set; }

        public ApiWithErrorModel(ILogger<ApiWithErrorModel> logger, HtmlEncoder encoder)
        {
            _logger = logger;
            _encoder = encoder;
        }

        public async Task OnGet([HttpClientName("github")] HttpClient client)
        {
            var uri = "/some-fake-api";

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    await client.GetStringAsync(uri);
                }
                catch (BrokenCircuitException ex)
                {
                    //Circuit has been tripped. Send some default data or otherwise handle the fact that your thing is broken.
                    _logger.LogError(ex, "Circuit broken");

                    await Task.Delay(7000);
                    uri = "/";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "error calling service");
                }
            }
        }
    }
}
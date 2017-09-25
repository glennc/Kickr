using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Kickr.Mvc;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace sample.Pages
{
    public class AnotherApiModel : PageModel
    {
        private ILogger<AnotherApiModel> _logger;
        private HtmlEncoder _encoder;

        public IHtmlContent Payload { get; set; }

        public AnotherApiModel(ILogger<AnotherApiModel> logger, HtmlEncoder encoder)
        {
            _logger = logger;
            _encoder = encoder;
        }

        public async Task OnGet([HttpClientName("github")] HttpClient client)
        {
            Payload = new HtmlString(_encoder.Encode(await client.GetStringAsync("/")));

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
}

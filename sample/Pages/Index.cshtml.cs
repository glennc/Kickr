using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Kickr;

namespace sample.Pages
{
    public class IndexModel : PageModel
    {
        private ILogger<IndexModel> _logger;
        private IHttpClientFactory _factory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        public async Task OnGet()
        {
            var client = _factory.GetHttpClient();
            var s = await client.GetStringAsync("http://www.google.com");
        }
    }


}

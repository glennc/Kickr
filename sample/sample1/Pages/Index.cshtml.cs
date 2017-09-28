using System;
using System.Threading.Tasks;
using Kickr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace sample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IServiceDiscoveryClient _client;

        public IndexModel(ILogger<IndexModel> logger, IServiceDiscoveryClient client)
        {
            _logger = logger;
            _client = client;
        }

        public Uri BaseAddress { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            BaseAddress = await _client.GetUriAsync(new Uri("http://sample"));
            return Page();
        }
    }
}

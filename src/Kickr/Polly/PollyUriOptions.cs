using System.Net.Http;
using Polly;

namespace Kickr.Policy
{
    public class PollyUriOptions
    {
        public Policy<HttpResponseMessage> Policy { get; set; }
    }
}
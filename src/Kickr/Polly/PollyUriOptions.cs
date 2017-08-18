

using Polly;
using System;
using System.Net.Http;

namespace Kickr.Policy
{
    public class PollyUriOptions
    {
        public Policy<HttpResponseMessage> Policy { get; set; }
    }
}
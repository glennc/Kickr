using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Kickr.Policy
{
    public static class PolicyBuilderExtensions
    {
        public static PolicyBuilder AddCircuitBreaker(this PolicyBuilder builder, int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            var defaultBreaker = Polly.Policy
                                    .Handle<HttpRequestException>()
                                    .OrResult<HttpResponseMessage>(m => m.IsSuccessStatusCode)
                                    .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking, durationOfBreak);
            builder.AddPolicy(defaultBreaker);
            return builder;
        }
    }
}

using System;
using System.Net.Http;
using Kickr.Policy;
using Polly;

namespace Kickr
{
    public static class PollyPolicyBuilderExtensions
    {
        public static PollyPolicyBuilder AddCircuitBreaker(this PollyPolicyBuilder builder, int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            var defaultBreaker = Polly.Policy
                                    .Handle<HttpRequestException>()
                                    .OrResult<HttpResponseMessage>(m => !m.IsSuccessStatusCode)
                                    .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking, durationOfBreak);
            builder.AddPolicy(defaultBreaker);
            return builder;
        }

        public static PollyPolicyBuilder AddRetry(this PollyPolicyBuilder builder)
        {
            var defaultRetry = Polly.Policy
                                    .Handle<HttpRequestException>()
                                    .OrResult<HttpResponseMessage>(m => !m.IsSuccessStatusCode)
                                    .RetryAsync();

            builder.AddPolicy(defaultRetry);

            return builder;
        }

        public static PollyPolicyBuilder AddRetry(this PollyPolicyBuilder builder, int timesToRetry)
        {
            var defaultRetry = Polly.Policy
                                    .Handle<HttpRequestException>()
                                    .OrResult<HttpResponseMessage>(m => !m.IsSuccessStatusCode)
                                    .RetryAsync(timesToRetry);

            builder.AddPolicy(defaultRetry);

            return builder;
        }

    }
}

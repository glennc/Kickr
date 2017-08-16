using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Kickr.Policy
{
    public static class PollyPolicyBuilderExtensions
    {
        public static PollyPolicyBuilder AddCircuitBreaker(this PollyPolicyBuilder builder, int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            var defaultBreaker = Polly.Policy
                                    .Handle<HttpRequestException>()
                                    .OrResult<HttpResponseMessage>(m => m.IsSuccessStatusCode)
                                    .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking, durationOfBreak);
            builder.AddPolicy(defaultBreaker);
            return builder;
        } 

        public static PollyPolicyBuilder AddRetry(this PollyPolicyBuilder builder)
        {
            var defaultRetry = Polly.Policy
                                    .Handle<HttpRequestException>()
                                    .OrResult<HttpResponseMessage>(m => m.IsSuccessStatusCode)
                                    .RetryAsync();

            builder.AddPolicy(defaultRetry);

            return builder;
		}

		public static PollyPolicyBuilder AddRetry(this PollyPolicyBuilder builder, int timesToRetry)
		{
			var defaultRetry = Polly.Policy
									.Handle<HttpRequestException>()
									.OrResult<HttpResponseMessage>(m => m.IsSuccessStatusCode)
									.RetryAsync(timesToRetry);

			builder.AddPolicy(defaultRetry);

			return builder;
		}

    }
}

using Polly;
using Polly.CircuitBreaker;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Kickr.Tests
{
    public class Polly
    {
        [Fact]
        public void breaker_policies_are_not_shared()
        {
            var pollyService = new PolicyService(_ => Policy
                                                       .Handle<Exception>()
                                                       .OrResult<HttpResponseMessage>(__ => true)
                                                       .CircuitBreaker(1, TimeSpan.FromSeconds(30)));

            var breaker1 = (CircuitBreakerPolicy<HttpResponseMessage>)pollyService.GetPolicy(new Uri("http://policy1"));
            var breaker2 = (CircuitBreakerPolicy<HttpResponseMessage>)pollyService.GetPolicy(new Uri("http://policy2"));

            Assert.Throws<Exception>(() => breaker1.Execute(() => throw new Exception()));
            Assert.Throws<BrokenCircuitException>(() => breaker1.Execute(() => throw new Exception()));

            Assert.NotEqual(breaker1, breaker2);

            Assert.Equal(breaker1.CircuitState, CircuitState.Open);
            Assert.Equal(breaker2.CircuitState, CircuitState.Closed);
        }

        [Fact]
        public async Task can_combine_retry_and_breaker()
        {
            var pollyService = new PolicyService(_ =>
            {
                var retry = Policy
                                .Handle<Exception>()
                                .OrResult<HttpResponseMessage>(x=>false)
                                .RetryAsync();
                var breaker = Policy
                                .Handle<Exception>()
                                .OrResult<HttpResponseMessage>(x=>false)
                                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

                return Policy.WrapAsync(retry, breaker);
            });

            var count = 0;
            var policy = pollyService.GetPolicy(new Uri("http://test"));
            await Assert.ThrowsAsync<Exception>(async () => await policy.ExecuteAsync(() => { count++; throw new Exception(); }));
            Assert.Equal(2, count);
            await Assert.ThrowsAsync<Exception>(async () => await policy.ExecuteAsync(() => { count++; throw new Exception(); }));
            await Assert.ThrowsAsync<BrokenCircuitException>(async () => await policy.ExecuteAsync(() => { count++; throw new Exception(); }));

            //The retry wouuld've set count to 6, but the circuit breaker tripping at five would cause it to not do the last one.
            Assert.Equal(5, count);
        }
    }
}

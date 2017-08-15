using Polly;
using Polly.CircuitBreaker;
using System;
using System.Net.Http;
using Xunit;

namespace Kickr.Tests
{
    public class Polly
    {
        [Fact]
        public void breaker_policies_are_not_shared()
        {
            var polyService = new PolicyService(_ => Policy
                                                       .Handle<Exception>()
                                                       .OrResult<HttpResponseMessage>(__ => true)
                                                       .CircuitBreaker(1, TimeSpan.FromSeconds(30)));

            var breaker1 = (CircuitBreakerPolicy<HttpResponseMessage>)polyService.GetPolicy(new Uri("http://policy1"));
            var breaker2 = (CircuitBreakerPolicy<HttpResponseMessage>)polyService.GetPolicy(new Uri("http://policy2"));

            Assert.Throws<Exception>(() => breaker1.Execute(() => throw new Exception()));
            Assert.Throws<BrokenCircuitException>(() => breaker1.Execute(() => throw new Exception()));

            Assert.NotEqual(breaker1, breaker2);

            Assert.Equal(breaker1.CircuitState, CircuitState.Open);
            Assert.Equal(breaker2.CircuitState, CircuitState.Closed);
        }
    }
}

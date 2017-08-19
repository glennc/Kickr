using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Kickr.Tests
{
    public class Header
    {
        [Fact]
        public void headers_override()
        {
            var services = new ServiceCollection()
                                .AddOptions()
                                .AddHeaders(p => p.AddHeaders(o => o.Headers.Add("Key1", ""))
                                                  .AddHeaders(o => o.Headers.Add("Key2", ""))
                                                  .AddHeaders("http://Uri", o => o.Headers.Add("Key3", "")))
                                .BuildServiceProvider();


            var monitor = services.GetRequiredService<IOptionsMonitor<HeaderOptions>>();
            Assert.Equal(3, monitor.Get("http://Uri").Headers.Count);
            Assert.Equal(2, monitor.CurrentValue.Headers.Count);
            Assert.Equal(2, monitor.Get("nonexistantkey").Headers.Count);
        }

        [Fact]
        public void multi_value_headers_work()
        {
            var services = new ServiceCollection()
                               .AddOptions()
                               .AddHttpClientFactory(p =>
                               {
                                   p.UseHeaders();
                               })
                               .AddHeaders(p => p.AddHeaders(o => o.Headers.Add("Key1", ""))
                                                 .AddHeaders(o => o.Headers.Add("Key2", "value1"))
                                                 .AddHeaders(o => o.Headers.Add("Key2", "value2"))
                                                 .AddHeaders("http://Uri", o => o.Headers.Add("Key3", "")));

            //Because send is protected it seemed easier to just run the whole test through httpClient
            //replacing the last handler instead of isolating it to just executing send on a single handler.
            var fakeHandler = new FakeHandler(req =>
            {
                Assert.True(req.Headers.Contains("key1"));
                Assert.Equal(3, req.Headers.GetValues("Key2").Count());
            });

            services.AddSingleton<HttpClientPipelineBuilder>(new FakeHttpPipelineBuilder(services, fakeHandler));

            var provider = services.BuildServiceProvider();

            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var client = httpClientFactory.GetHttpClient();

            var result = client.GetAsync("http://about");
        }

        private class FakeHttpPipelineBuilder : HttpClientPipelineBuilder
        {
            public FakeHttpPipelineBuilder(IServiceCollection services, HttpMessageHandler baseHandler) : base(services)
            {
                BaseHandler = baseHandler;
            }
        }


        private class FakeHandler : HttpClientHandler
        {
            private Action<HttpRequestMessage> _verifyAction;

            public FakeHandler(Action<HttpRequestMessage> action)
            {
                _verifyAction = action;
            }
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage());
            }
        }
    }
}

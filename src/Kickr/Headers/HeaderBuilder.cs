using System;
using Microsoft.Extensions.DependencyInjection;

namespace Kickr
{
    public class HeaderBuilder
    {
        private IServiceCollection _services;

        public HeaderBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public HeaderBuilder AddHeaders(Action<HeaderOptions> optionsBuilder)
        {
            _services.ConfigureAll(optionsBuilder);
            return this;
        }

        public HeaderBuilder AddHeaders(string uriKey, Action<HeaderOptions> optionsBuilder)
        {
            _services.Configure(uriKey, optionsBuilder);
            return this;
        }
    }

    public static class HeaderBuilderExtension
    {
        public static HttpClientPipelineBuilder UseHeaders(this HttpClientPipelineBuilder pipeline)
        {
            pipeline.AddHandler<HeaderHandler>();
            return pipeline;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Kickr.Options
{
    public class HttpClientPipelineBuilder
    {
        public IServiceCollection Services { get; private set; }
        public List<Func<HttpClientHandler, DelegatingHandler>> HandlerPipeline { get; private set; }

        public HttpClientPipelineBuilder(IServiceCollection services)
        {
            Services = services;
            HandlerPipeline = new List<Func<HttpClientHandler, DelegatingHandler>>();
        }

        public HttpClientHandler Build()
        {

        }
    }
}

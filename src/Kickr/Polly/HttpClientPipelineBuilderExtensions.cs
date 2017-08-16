using System;
using System.Collections.Generic;
using System.Text;
using Kickr.Policy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;

namespace Kickr
{
    public static class HttpClientPipelineBuilderExtensions
    {
        public static HttpClientPipelineBuilder UsePolly(this HttpClientPipelineBuilder builder, Action<PollyHttpHandlerBuilder> pollyBuilder)
        {

            //TODO: Add HealthCheck to DI. Make sure that adding it to DI is enough to make it work.
            var p = new PollyHttpHandlerBuilder(builder.Services);
            pollyBuilder(p);
            builder.AddHandler<PollyHttpMessageHandler>();
            return builder;
        }
    }
}

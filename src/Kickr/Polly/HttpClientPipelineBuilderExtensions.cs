using Kickr.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kickr.Policy
{
    public static class HttpClientPipelineBuilderExtensions
    {
        public static HttpClientPipelineBuilder UsePolly(this HttpClientPipelineBuilder builder, Action<PollyHttpHandlerBuilder> pollyBuilder)
        {
            var p = new PollyHttpHandlerBuilder(builder.Services);
            pollyBuilder(p);
            builder.HandlerPipeline.Add(p.Build());
            return builder;
        }
    }
}

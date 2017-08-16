using Kickr.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Kickr.Policy;

namespace Kickr
{
    public static class HttpClientPipelineBuilderExtensions
    {
        public static HttpClientPipelineBuilder UsePolly(this HttpClientPipelineBuilder builder, Action<PollyHttpHandlerBuilder> pollyBuilder)
        {
            var p = new PollyHttpHandlerBuilder(builder.Services);
            pollyBuilder(p);
            builder.AddHandler<PollyHttpMessageHandler>();
            return builder;
        }
    }
}

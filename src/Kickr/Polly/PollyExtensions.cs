
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
        public static HttpClientPipelineBuilder UsePolly(this HttpClientPipelineBuilder builder)
        {
			//TODO: Add HealthCheck to DI. Make sure that adding it to DI is enough to make it work.
			builder.AddHandler<PollyHttpMessageHandler>();
            return builder;
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPolly(this IServiceCollection services, Action<PollyHttpHandlerBuilder> pollyBuilder)
        {
			var p = new PollyHttpHandlerBuilder(services);
			pollyBuilder(p);
            return services;
        }
    }
}

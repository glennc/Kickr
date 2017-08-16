using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Consul;
using Kickr;
using Kickr.Consul;
using Kickr.Policy;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static HttpClientPipelineBuilder AddKickr(this IServiceCollection services)
        {
            services.TryAddScoped<IHttpClientFactory, HttpClientFactory>();

            var pipelineBuilder = new HttpClientPipelineBuilder(services);
            services.TryAddSingleton(pipelineBuilder);
            return pipelineBuilder;
        }

    }
}

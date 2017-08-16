using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Kickr.Options;
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
            services.AddSingleton<IUriPolicyService, PolicyService>();
            services.AddSingleton<PolicyCheck>();
            services.AddHealthChecks(check => check.AddCheck<PolicyCheck>("PolicyCheck", TimeSpan.FromSeconds(10)));
            services.AddScoped<IHttpClientFactory, HttpClientFactory>();

            var pipelineBuilder = new HttpClientPipelineBuilder(services);
            services.AddSingleton(pipelineBuilder);
            return pipelineBuilder;
        }

    }
}

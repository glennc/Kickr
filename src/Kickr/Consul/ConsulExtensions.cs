using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.HealthChecks;

namespace Kickr.Consul
{
    public static class ConsulExtensions
    {
        public static HttpClientPipelineBuilder UseConsulServiceDiscovery(this HttpClientPipelineBuilder builder)
        {
            builder.Services.TryAddSingleton<IConsulClient, ConsulClient>();
            builder.Services.TryAddSingleton<IServiceDiscoveryClient, ConsulServiceDiscoveryClient>();
            builder.Services.TryAddSingleton<IHostedService, ConsulRegistrar>();

            builder.AddHandler<ConsulMessageHandler>();

            return builder;
        }
    }
}

using Consul;
using Kickr.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kickr.Consul
{
    public static class ConsulExtensions
    {
        public static HttpClientPipelineBuilder UseConsulServiceDiscovery(this HttpClientPipelineBuilder builder)
        {
            builder.Services.AddSingleton<IConsulClient, ConsulClient>();
            builder.Services.AddSingleton<IServiceDiscoveryClient, ConsulServiceDiscoveryClient>();
            builder.Services.AddSingleton<IHostedService, ConsulRegistrar>();

            return builder;
        }
    }
}

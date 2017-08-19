using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Kickr.Consul
{
    public static class ConsulExtensions
    {
        public static HttpClientPipelineBuilder UseConsulServiceDiscovery(this HttpClientPipelineBuilder builder)
        {
            builder.AddHandler<ConsulMessageHandler>();
            return builder;
        }

        public static IServiceCollection AddConsul(this IServiceCollection services)
        {
            services.TryAddSingleton<IConsulClient, ConsulClient>();
            services.TryAddSingleton<IServiceDiscoveryClient, ConsulServiceDiscoveryClient>();
            services.TryAddSingleton<IHostedService, ConsulRegistrar>();
            return services;
        }
    }
}

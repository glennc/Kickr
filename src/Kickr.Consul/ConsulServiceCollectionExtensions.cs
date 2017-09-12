using Consul;
using Kickr.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Kickr.Consul
{
    public static class ConsulServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services)
        {
            services.TryAddSingleton<IConsulClient, ConsulClient>();
            services.TryAddSingleton<IServiceDiscoveryClient, ConsulServiceDiscoveryClient>();
            services.TryAddSingleton<IHostedService, ConsulRegistrar>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, ConsulHttpClientFactoryPolicy>());

            return services;
        }
    }
}

using System;
using Kickr;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClientFactory(this IServiceCollection services, Action<HttpClientPipelineBuilder> pipelineBuilder)
        {
            services.TryAddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddSingleton<IUriKeyGenerator>(new FuncUriKeyGenerator(uri => uri.Host));

            var pipeline = new HttpClientPipelineBuilder(services);
            pipelineBuilder(pipeline);
            services.TryAddSingleton(pipeline);
            return services;
        }

        public static IServiceCollection AddHttpClientFactory(this IServiceCollection services)
        {
            return services.AddHttpClientFactory(_ => { });
        }

    }
}

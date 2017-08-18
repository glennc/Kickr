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

        public static IServiceCollection AddHttpClientFactory(this IServiceCollection services, Action<HttpClientPipelineBuilder> pipelineBuilder)
        {
            services.TryAddScoped<IHttpClientFactory, HttpClientFactory>();

            var pipeline = new HttpClientPipelineBuilder(services);
            pipelineBuilder(pipeline);
            services.TryAddSingleton(pipeline);
            return services;
        }

		public static IServiceCollection AddHttpClientFactory(this IServiceCollection services)
		{
			services.TryAddScoped<IHttpClientFactory, HttpClientFactory>();

			var pipeline = new HttpClientPipelineBuilder(services);
			services.TryAddSingleton(pipeline);
			return services;
		}

    }
}

using System;
using Kickr.Refit;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RestClientServiceCollectionExtensions
    {
        public static IServiceCollection AddRestClient(this IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IRestClient<>), typeof(RestClient<>));
            return services;
        }

        public static IServiceCollection AddRestClient<TClient>(this IServiceCollection services, Action<NamedClientOptions> action)
        {
            services.AddRestClient();
            services.Configure(typeof(TClient).Name, action);
            return services;
        }

        public static IServiceCollection AddRestClient<TClient>(this IServiceCollection services, string uri)
        {
            services.AddRestClient<TClient>(o => o.BaseAddress = new Uri(uri));
            return services;
        }
    }
}

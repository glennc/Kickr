using System;
using Kickr.Refit;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RestCilentServiceCollectionExtensions
    {
		public static IServiceCollection AddRestClient<TClient>(this IServiceCollection services, Action<RestOptions> action)
		{
			services.TryAddSingleton(typeof(IRestClient<>), typeof(RestClient<>));
			services.Configure(nameof(TClient), action);
			return services;
		}

		public static IServiceCollection AddRestClient<TClient>(this IServiceCollection services, string uri)
		{
            services.AddRestClient<TClient>(o => o.Uri = uri);
			return services;
		}
    }
}

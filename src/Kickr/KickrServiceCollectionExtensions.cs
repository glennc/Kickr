using System;
using Kickr;
using Kickr.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KickrServiceCollectionExtensions
    {
        public static IServiceCollection AddKickr(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<HttpClientFactory, DefaultHttpClientFactory>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, DefaultHeaderHttpClientFactoryPolicy>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, NamedClientBaseAddessHttpClientFactoryPolicy>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, LoggingScopeHttpClientFactoryPolicy>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, MessageLoggingHttpClientFactoryPolicy>());

            return services;
        }

        public static IServiceCollection AddKickrGlobalHeaders(this IServiceCollection services, Action<HeaderOptions> action)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            services.ConfigureAll<HeaderOptions>(action);
            return services;
        }

        public static IServiceCollection AddKickrHeaders(this IServiceCollection services, string name, Action<HeaderOptions> action)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            services.Configure<HeaderOptions>(name, action);
            return services;
        }
    }
}

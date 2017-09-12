
using System;
using System.Net.Http;
using Kickr.Http;
using Kickr.Polly;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPolly(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpClientFactoryPolicy, PollyHttpClientFactoryPolicy>());
            return services;
        }

        public static IServiceCollection AddKickrGlobalPolicy(this IServiceCollection services, Func<global::Polly.PolicyBuilder<HttpResponseMessage>, global::Polly.Policy<HttpResponseMessage>> action)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = global::Polly.Policy<HttpResponseMessage>.Handle<HttpRequestException>().OrResult(m => !m.IsSuccessStatusCode);
            services.ConfigureAll<PollyOptions>(options => options.Policies.Add(action(builder)));
            return services;
        }

        public static IServiceCollection AddKickrPolicy(this IServiceCollection services, string name, Func<global::Polly.PolicyBuilder<HttpResponseMessage>, global::Polly.Policy<HttpResponseMessage>> action)
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

            var builder = global::Polly.Policy<HttpResponseMessage>.Handle<HttpRequestException>().OrResult(m => !m.IsSuccessStatusCode);
            services.Configure<PollyOptions>(name, options => options.Policies.Add(action(builder)));
            return services;
        }
    }
}

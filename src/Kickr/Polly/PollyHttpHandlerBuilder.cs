using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Kickr.Policy
{
    public class PollyHttpHandlerBuilder
    {
        public IServiceCollection Services { get; private set; }

        public PollyHttpHandlerBuilder(IServiceCollection services)
        {
            this.Services = services;
        }

        public PollyHttpHandlerBuilder ConfigureUri(string name, Action<PolicyBuilder> action)
        {
            return ConfigureUri(new Uri(name), action);
        }

        public PollyHttpHandlerBuilder ConfigureUri(Uri name, Action<PolicyBuilder> action)
        {
            Configure($"{name.Host}:{name.Port}", name, action);
            return this;
        }

        public PollyHttpHandlerBuilder ConfigureDefaultPolicy(Action<PolicyBuilder> action)
        {
            Configure("Default", null, action);
            return this;
        }

        private void Configure(string name, Uri uri, Action<PolicyBuilder> action)
        {
            var policyBuilder = new PolicyBuilder();
            action(policyBuilder);
            var policy = policyBuilder.Build();
            Services.Configure<PollyUriOptions>(name, o =>
            {
                o.Uri = uri;
                o.Policy = policy;
            });
        }
    }
}
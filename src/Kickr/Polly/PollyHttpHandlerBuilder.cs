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

        public PollyHttpHandlerBuilder ConfigureUri(string name, Action<PollyPolicyBuilder> action)
        {
            return ConfigureUri(new Uri(name), action);
        }

        public PollyHttpHandlerBuilder ConfigureUri(Uri name, Action<PollyPolicyBuilder> action)
        {
            Configure($"{name.Host}:{name.Port}", name, action);
            return this;
        }

        public PollyHttpHandlerBuilder ConfigureDefaultPolicy(Action<PollyPolicyBuilder> action)
        {
            Configure("Default", null, action);
            return this;
        }

        private void Configure(string name, Uri uri, Action<PollyPolicyBuilder> action)
        {
            var policyBuilder = new PollyPolicyBuilder();
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
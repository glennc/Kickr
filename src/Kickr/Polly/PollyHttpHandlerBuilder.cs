using Microsoft.Extensions.DependencyInjection;
using Polly;
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

        public PollyHttpHandlerBuilder UsePolicy(string uriKey, Action<PollyPolicyBuilder> action)
        {
            var policy = Configure(action);
            Services.Configure<PollyUriOptions>(uriKey, o =>
            {
                o.Policy = policy;
            });
            return this;
        }

        public PollyHttpHandlerBuilder UsePolicy(Action<PollyPolicyBuilder> action)
        {
            var policy = Configure(action);
            Services.ConfigureAll<PollyUriOptions>(o =>
            {
                o.Policy = policy;
            });
            return this;
        }

        private Policy<HttpResponseMessage> Configure(Action<PollyPolicyBuilder> action)
        {
            var policyBuilder = new PollyPolicyBuilder();
            action(policyBuilder);
            return policyBuilder.Build();
        }
    }
}
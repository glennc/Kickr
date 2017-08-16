using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Kickr.Policy
{
    public class PollyPolicyBuilder
    {
        private List<Policy<HttpResponseMessage>> _policies;

        public PollyPolicyBuilder()
        {
            _policies = new List<Policy<HttpResponseMessage>>();
        }

        public PollyPolicyBuilder AddPolicy(Policy<HttpResponseMessage> policy)
        {
            _policies.Add(policy);
            return this;
        }

        public Policy<HttpResponseMessage> Build()
        {

            if (_policies.Count() > 1)
            {
                return Polly.Policy.WrapAsync(_policies.ToArray());
            }

            return _policies[0];
        }
    }
}
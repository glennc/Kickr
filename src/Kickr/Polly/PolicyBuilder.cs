using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Kickr.Policy
{
    public class PolicyBuilder
    {
        private List<Policy<HttpResponseMessage>> _policies;

        public PolicyBuilder()
        {
            _policies = new List<Policy<HttpResponseMessage>>();
        }

        public PolicyBuilder AddPolicy(Policy<HttpResponseMessage> policyFunc)
        {
            _policies.Add(policyFunc);
            return this;
        }

        public Policy<HttpResponseMessage> Build()
        {

            if (_policies.Count() > 1)
            {
                return Polly.Policy.Wrap(_policies.ToArray());
            }

            return _policies[0];
        }
    }
}
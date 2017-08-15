using System;
using System.Collections.Generic;
using System.Text;
using Polly;
using System.Collections.Concurrent;
using System.Net.Http;

namespace Kickr
{
    public class PolicyService : IUriPolicyService
    {
        private const int NUMBER_OF_ALLOWED_ERRORS = 3;

        private ConcurrentDictionary<string, Policy<HttpResponseMessage>> _policies;
        private Func<Policy<HttpResponseMessage>> _defaultPolicyBuilder;
        private static Policy<HttpResponseMessage> DefaultPolicy => Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>((resp) =>
                {
                    return resp.IsSuccessStatusCode;
                })
                .CircuitBreakerAsync(NUMBER_OF_ALLOWED_ERRORS, TimeSpan.FromSeconds(30));

        public PolicyService()
            : this(() => DefaultPolicy)
        {

        }

        public PolicyService(Func<Policy<HttpResponseMessage>> defaultPolicyBuilder)
        {
            _policies = new ConcurrentDictionary<string, Policy<HttpResponseMessage>>();
            _defaultPolicyBuilder = defaultPolicyBuilder;
        }


        public IEnumerable<KeyValuePair<string, Policy<HttpResponseMessage>>> GetPolicies()
        {
            return _policies.ToArray();
        }

        public Policy<HttpResponseMessage> GetPolicy(Uri policyName)
        {
            Policy<HttpResponseMessage> policy;
            if(!_policies.TryGetValue(policyName.ToString(), out policy))
            {
                policy = _defaultPolicyBuilder();
                //TODO: Put some more thought into this. After a certain point for most applications this should be almost all reads.
                _policies.TryAdd(policyName.ToString(), policy);
            }
            return policy;
        }
    }
}

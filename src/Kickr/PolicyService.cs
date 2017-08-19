using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using Polly;

namespace Kickr
{
    public class PolicyService : IUriPolicyService
    {
        private const int NUMBER_OF_ALLOWED_ERRORS = 3;

        private ConcurrentDictionary<string, Policy<HttpResponseMessage>> _policies = new ConcurrentDictionary<string, Policy<HttpResponseMessage>>();

        private Func<Uri, Policy<HttpResponseMessage>> _defaultPolicyBuilder;

        private static Policy<HttpResponseMessage> DefaultPolicy => Polly.Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>((resp) =>
                {
                    return resp.IsSuccessStatusCode;
                })
                .CircuitBreakerAsync(NUMBER_OF_ALLOWED_ERRORS, TimeSpan.FromSeconds(30));

        public PolicyService()
            : this(_ => DefaultPolicy)
        {

        }

        public PolicyService(Func<Uri, Policy<HttpResponseMessage>> defaultPolicyBuilder)
        {
            _defaultPolicyBuilder = defaultPolicyBuilder;
        }

        public IEnumerable<KeyValuePair<string, Policy<HttpResponseMessage>>> GetPolicies()
        {
            return _policies.ToArray();
        }

        public Policy<HttpResponseMessage> GetPolicy(Uri policyName)
        {
            if (!_policies.TryGetValue(policyName.ToString(), out var policy))
            {
                policy = _defaultPolicyBuilder(policyName);
                //TODO: Put some more thought into this. After a certain point for most applications this should be almost all reads.
                _policies.TryAdd(policyName.ToString(), policy);
            }
            return policy;
        }
    }
}

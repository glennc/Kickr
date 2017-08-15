using Microsoft.Extensions.HealthChecks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kickr.Policy
{
    public class PolicyCheck : IHealthCheck
    {
        private IUriPolicyService _policyService;

        public PolicyCheck(IUriPolicyService policyService)
        {
            _policyService = policyService;
        }

        public async ValueTask<IHealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var policies = _policyService.GetPolicies();

            var description = new StringBuilder();

            foreach(var policy in policies)
            {
                var circuitBreakerPolicy = (Polly.CircuitBreaker.CircuitBreakerPolicy<HttpResponseMessage>)policy.Value;
                description.Append($"\nUri: {policy.Key}, Status: {circuitBreakerPolicy.CircuitState}");
            }

            return HealthCheckResult.FromStatus(CheckStatus.Healthy, description.ToString());
        }
    }
}

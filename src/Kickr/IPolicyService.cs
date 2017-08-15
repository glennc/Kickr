using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Kickr
{
    public interface IUriPolicyService
    {
        Policy<HttpResponseMessage> GetPolicy(Uri policyName);

        IEnumerable<KeyValuePair<string, Policy<HttpResponseMessage>>> GetPolicies();
    }
}

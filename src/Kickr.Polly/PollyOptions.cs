
using Polly;

namespace Microsoft.Extensions.DependencyInjection
{
    public class PollyOptions
    {
        public PolicyCollection Policies { get; } = new PolicyCollection();
    }
}
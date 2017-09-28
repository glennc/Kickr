
using System.Net.Http;

namespace Kickr
{
    public abstract class HttpClientFactory
    {
        public static readonly string DefaultClientName = "Default";

        public virtual HttpClient GetDefaultClient()
        {
            return GetNamedClient(DefaultClientName);
        }

        public abstract HttpClient GetNamedClient(string name);
    }
}

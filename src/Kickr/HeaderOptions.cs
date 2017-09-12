using Microsoft.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public class HeaderOptions
    {
        public IHeaderDictionary Headers { get; } = new HeaderDictionary();
    }
}

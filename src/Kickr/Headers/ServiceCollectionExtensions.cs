using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kickr
{
    public static class HeaderServiceCollectionExtensions
    {
        public static IServiceCollection AddHeaders(this IServiceCollection services, Action<HeaderBuilder> headerBuilder)
        {
            var h = new HeaderBuilder(services);
            headerBuilder(h);
            return services;
        }
    }
}

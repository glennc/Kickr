using System;
using Microsoft.Extensions.DependencyInjection;

namespace Kickr.Headers
{
    public class HeaderBuilder
    {
        private IServiceCollection _services;

        public HeaderBuilder(IServiceCollection services)
        {
            _services = services;
        }

    }
}

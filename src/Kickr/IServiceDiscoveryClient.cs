using System;
using System.Collections.Generic;
using System.Text;

namespace Kickr
{
    public interface IServiceDiscoveryClient
    {
        Uri GetUrl(Uri service);
    }
}

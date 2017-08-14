using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kickr
{
    public interface IServiceDiscoveryClient
    {
        Task<Uri> GetUrl(Uri service);
        Task<Uri> GetUrl(Uri service, CancellationToken token);
    }
}

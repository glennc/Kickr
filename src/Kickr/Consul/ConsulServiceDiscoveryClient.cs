using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;

namespace Kickr.Consul
{
    public class ConsulServiceDiscoveryClient : IServiceDiscoveryClient
    {
        private IConsulClient _client;

        public ConsulServiceDiscoveryClient(IConsulClient client)
        {
            _client = client;
        }

        public async Task<Uri> GetUrl(Uri service, CancellationToken token)
        {
            var result = await _client.Catalog.Service(service.Host, token);

            if(result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ConsulRequestException("Non-successs status code from Consul during service discovery.", result.StatusCode);
            }

            if (result.Response.Any())
            {
                //TODO: Do we need our own load balancing policy when not using DNS?
                var serviceToRouteTo = result.Response.First();
                var existingUri = service.ToString();

                //TODO: might need to be more advanced about this. For example, if a service URI contains a port
                //that is not the port of the service should it throw?
                var builder = new UriBuilder
                {
                    Host = serviceToRouteTo.Address,
                    Path = service.AbsolutePath,
                    Port = serviceToRouteTo.ServicePort,
                    Query = service.Query,
                    Scheme = service.Scheme
                };
                return new Uri(builder.ToString());
            }
            else
            {
                return service;
            }
        }

        public async Task<Uri> GetUrl(Uri service)
        {
            return await this.GetUrl(service, default(CancellationToken));
        }
    }
}

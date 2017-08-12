using Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kickr.Consul
{
    public class ConsulRegistrar : IHostedService
    {
        private IHostingEnvironment _env;
        private IServer _server;
        private ILogger<ConsulRegistrar> _logger;

        public ConsulRegistrar(IHostingEnvironment env, IServer server, ILogger<ConsulRegistrar> logger)
        {
            _env = env;
            _server = server;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var client = new ConsulClient())
            {
                var addressFeature = _server.Features.Get<IServerAddressesFeature>();
                //TODO: on second thought this loop is a bad idea.
                foreach (var address in addressFeature.Addresses)
                {
                    var uri = new Uri(address);

                    try
                    {
                        var result = await client.Agent.ServiceRegister(new AgentServiceRegistration
                        {
                            Name = _env.ApplicationName,
                            Address = address,
                            Check = new AgentServiceCheck {
                                HTTP = address,
                                Interval = TimeSpan.FromSeconds(5)
                            }
                        }, cancellationToken);

                        if (result.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            throw new ApplicationException("Service registration not accepted.");
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, $"Error registering service for {_env.ApplicationName}");
                        throw ex;
                    }
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using (var client = new ConsulClient())
            {
                try
                {
                    await client.Agent.ServiceDeregister(_env.ApplicationName, cancellationToken);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Error de-registering service for {_env.ApplicationName}");
                    throw ex;
                }
            }
        }
    }
}

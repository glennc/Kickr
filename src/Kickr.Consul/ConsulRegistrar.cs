using System;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kickr.Consul
{
    public class ConsulRegistrar : IHostedService
    {
        private IHostingEnvironment _env;
        private IServer _server;
        private ILogger<ConsulRegistrar> _logger;
        private IHealthCheckService _healthChecks;
        private bool _running = true;
        private string _ttlId;
        private Thread _workerThread;
        string _serviceId;

        public ConsulRegistrar(IHostingEnvironment env, IServer server, IHealthCheckService healthChecks, ILogger<ConsulRegistrar> logger)
        {
            _env = env;
            _server = server;
            _healthChecks = healthChecks;
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
                        _serviceId = _env.ApplicationName + Guid.NewGuid();
                        _ttlId = $"{_env.ApplicationName}_ttl_check_{Guid.NewGuid()}";

                        var result = await client.Agent.ServiceRegister(new AgentServiceRegistration
                        {
                            Name = _env.ApplicationName,
                            Address = address,
                            ID = _serviceId,
                            Port = uri.Port,
                            Checks = new AgentServiceCheck[] {
                                new AgentCheckRegistration
                                {
                                    Name = $"{_env.ApplicationName}_ping_check",
                                    ID = $"{_env.ApplicationName}_ping_check_{Guid.NewGuid()}",
                                    HTTP = address,
                                    Interval = TimeSpan.FromSeconds(20),
                                    Timeout = TimeSpan.FromSeconds(5)
                                }
                            }
                        }, cancellationToken);

                        if (result.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            throw new ApplicationException("Service registration not accepted.");
                        }

                        var checkResult = await client.Agent.CheckRegister(new AgentCheckRegistration
                        {
                            Name = $"{_env.ApplicationName}_ttl_check",
                            ID = _ttlId,
                            TTL = TimeSpan.FromSeconds(30),
                            ServiceID = _serviceId
                        });

                        if (result.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            throw new ApplicationException("Unable to register service TTL check.");
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error registering service for {_env.ApplicationName}");
                        throw ex;
                    }
                }
            }

            _workerThread = new Thread(new ThreadStart(RunTTLLoop2));
            _workerThread.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _running = false;
            _workerThread.Join(10);

            using (var client = new ConsulClient())
            {
                try
                {
                    await client.Agent.ServiceDeregister(_serviceId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error de-registering service for {_env.ApplicationName}");
                    throw ex;
                }
            }
        }

        private async void RunTTLLoop2()
        {
            DateTime lastRun = DateTime.UtcNow;

            while (_running)
            {
                if ((DateTime.UtcNow - lastRun).TotalSeconds >= 15)
                {
                    using (var client = new ConsulClient())
                    {
                        await client.Agent.PassTTL(_ttlId, "Alive");
                    }
                    lastRun = DateTime.UtcNow;
                }
                Thread.Sleep(1);
            }
        }

        private async void RunTTLLoop()
        {
            DateTime lastRun = DateTime.UtcNow;

            while (_running)
            {
                if ((DateTime.UtcNow - lastRun).TotalSeconds >= 15)
                {
                    var healthStatus = await _healthChecks.CheckHealthAsync();
                    using (var client = new ConsulClient())
                    {
                        switch (healthStatus.CheckStatus)
                        {
                            case CheckStatus.Healthy:
                                await client.Agent.PassTTL(_ttlId, healthStatus.Description);
                                break;
                            case CheckStatus.Unhealthy:
                                await client.Agent.FailTTL(_ttlId, healthStatus.Description);
                                break;
                            case CheckStatus.Warning:
                            case CheckStatus.Unknown:
                                await client.Agent.WarnTTL(_ttlId, healthStatus.Description);
                                break;
                        }
                    }
                    lastRun = DateTime.UtcNow;
                }
                Thread.Sleep(1);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Kickr;
using Kickr.Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.HealthChecks;
using Consul;
using Kickr.Checks;

namespace sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IConsulClient, ConsulClient>();
            services.AddSingleton<IUriPolicyService, PolicyService>();
            services.AddSingleton<IServiceDiscoveryClient, ConsulServiceDiscoveryClient>();
            services.AddSingleton<PolicyCheck>();
            services.AddHealthChecks(check => check.AddCheck<PolicyCheck>("PolicyCheck"));
            services.AddScoped<IHttpClientFactory, HttpClientFactory>();
            services.AddSingleton<IHostedService, ConsulRegistrar>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}

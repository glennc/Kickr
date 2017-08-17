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
using Polly;
using System.Net.Http;
using Kickr.Policy;

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

            services.AddConsul();

			services.AddPolly(p =>
			{
				p.ConfigureDefaultPolicy(o =>
				{
					o.AddCircuitBreaker(5, TimeSpan.FromSeconds(5));
				})

				.ConfigureUri("http://google.com", o =>
                {
                    o.AddRetry();
                    o.AddCircuitBreaker(2, TimeSpan.FromSeconds(5));
                });
			});

            services.AddHttpClientFactory(pipelineBuilder =>
                      pipelineBuilder
                        .UseConsulServiceDiscovery()
                        .UsePolly());
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

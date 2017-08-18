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

            //Not sure about how first class this needs to be. Probably depends on how advanced we make the default generator.
            services.AddSingleton<IUriKeyGenerator>(new FuncUriKeyGenerator(uri => uri.Host));

            services.AddPolly(p =>
			{
				p.UsePolicy(o =>
				{
					o.AddCircuitBreaker(5, TimeSpan.FromSeconds(5));
				})

				.UsePolicy("api.github.com", o =>
                {
                    o.AddRetry();
                    o.AddCircuitBreaker(1, TimeSpan.FromSeconds(5));
                });
			});

            services.AddHeaders(b =>
            {
                b.AddHeaders(o => o.Headers.Add("user-agent", "myagent"));
                b.AddHeaders("api.github.com", o => o.Headers.Add("Accept", "application/vnd.github.v3+json"));
            });

            services.AddHttpClientFactory(pipelineBuilder => pipelineBuilder
                        .UseHeaders()
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

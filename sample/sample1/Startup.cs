using System;
using Kickr;
using Kickr.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

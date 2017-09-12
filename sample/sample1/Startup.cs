using System;
using System.Net.Http;
using Kickr;
using Kickr.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

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
            services.AddMvc().AddKickr();

            services.AddKickr();
            services.AddConsul();
            services.AddPolly();

            services.AddKickrGlobalPolicy(b => b.CircuitBreakerAsync(5, TimeSpan.FromSeconds(5)));
            services.AddKickrPolicy("github", b => b.RetryAsync());
            services.AddKickrPolicy("github", b => b.CircuitBreakerAsync(1, TimeSpan.FromSeconds(5)));

            services.AddKickrGlobalHeaders(o => o.Headers.Add("user-agent", "myagent"));
            services.AddKickrHeaders("github", o => o.Headers.Add("Accept", "application/vnd.github.v3+json"));
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

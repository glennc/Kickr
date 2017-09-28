using System;
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
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddKickrModelBinder();

            services.AddKickr();
            services.AddConsul();
            services.AddPolly();

            services.AddKickrGlobalPolicy(b => b.CircuitBreakerAsync(5, TimeSpan.FromSeconds(5)));
            services.AddKickrGlobalHeaders(o => o.Headers.Add("user-agent", "Kickr-Sample"));

            services.AddKickrNamedClient("github", b => b.BaseAddress = new Uri("https://api.github.com"));
            services.AddKickrPolicy("github", b => b.RetryAsync());
            services.AddKickrPolicy("github", b => b.CircuitBreakerAsync(2, TimeSpan.FromSeconds(5)));
            services.AddKickrHeaders("github", o => o.Headers.Add("Accept", "application/vnd.github.v3+json"));
        }
        
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

            app.UseMvc();
        }
    }
}

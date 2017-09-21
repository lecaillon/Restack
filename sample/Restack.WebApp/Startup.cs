using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Restack.WebApp.Services;

namespace Restack.WebApp
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
            services.AddMvc()
                    .AddRestackMvc();

            services.AddRestack()
                    .AddPolly();

            services.AddRestackGlobalHeaders(o => o.Headers.Add("user-agent", "myagent"));

            services.AddRestClient<IGeoApi>("https://geo.api.gouv.fr")
                    .AddRestackHeaders<IGeoApi>(o => o.Headers.Add("api-key", "xxxxx-xxx-xxxxxxxx"))
                    .AddRestackPolicy<IGeoApi>(b => b.RetryAsync())
                    .AddRestackPolicy<IGeoApi>(b => b.CircuitBreakerAsync(1, TimeSpan.FromSeconds(5)));

            services.AddRestackHeaders("github", o => o.Headers.Add("Accept", "application/vnd.github.v3+json"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

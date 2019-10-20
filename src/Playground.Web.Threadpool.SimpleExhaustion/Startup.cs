using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Playground.Web.Threadpool.SimpleExhaustion
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/exhaust", ExhaustContext);
                endpoints.MapGet("/healthy", Healthy);
            });
        }

        public async Task ExhaustContext(HttpContext httpContext)
        {
            Console.WriteLine("Starting exhaust endpoint");
            
            await Task.Delay(1000 * 10);
            await Task.Delay(1000 * 3);
            await Task.Delay(1000 * 30);

            await httpContext.Response.WriteAsync("Hello World!");
        }

        public async Task Healthy(HttpContext httpContext)
        {
            await Task.Delay(30);
        }
    }
}

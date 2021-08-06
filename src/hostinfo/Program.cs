using System;
using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace hostinfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Program>();
            }).Build().Run();
        }

        public void ConfigureServices(IServiceCollection services ) { }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async ctx =>
                {
                    await ctx.Response.WriteAsync(
                        $"  ********** Version: V1 **********{Environment.NewLine}" +
                        $"  Framework - {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}{Environment.NewLine}" +
                        $"  OS - {System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier}{Environment.NewLine}" +
                        $"  Kernel - {System.Runtime.InteropServices.RuntimeInformation.OSDescription}{Environment.NewLine}" +
                        $"  Hostname - {Environment.MachineName}{Environment.NewLine}" +
                        $"  Host IP - {System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList.FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)}"
                    ).ConfigureAwait(false);
                });
            });
        }

    }
}

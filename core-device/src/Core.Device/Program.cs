using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Core.Device
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((hostingContext, builder) =>
             {
                 if (!hostingContext.HostingEnvironment.IsDevelopment())
                     builder.AddSecretsManager(configurator: options =>
                     {
                         options.KeyGenerator = (secret, name) => name.Replace("__", ":");
                     });
                 builder.AddEnvironmentVariables();
             })
                .UseSerilog((hostingContext, cfg) =>
                {
                    cfg.Enrich.FromLogContext();
                    cfg.Enrich.WithExceptionDetails();
                    cfg.Enrich.WithMachineName();
                    cfg.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning);
                    cfg.WriteTo.Console();
                })
                .UseStartup<Startup>();
    }
}

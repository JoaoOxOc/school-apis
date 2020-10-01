using SingleSignonPage.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Threading.Tasks;

namespace SingleSignonPage
{
    public class Program
    {
        static readonly LoggerProviderCollection Providers = new LoggerProviderCollection();
        public static void Main(string[] args)
        {
            Console.Title = "Chain of Change - Server SSO";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"chainchange_sso_log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .WriteTo.Providers(Providers)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            Task.WaitAll(DbMigrationHelpers.EnsureSeedData(host.Services.CreateScope()));

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                
                .ConfigureWebHostDefaults(webBuilder => 
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog(providers: Providers)
                    );
    }
}

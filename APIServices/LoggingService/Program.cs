using System;
using LoggingService.Code;
using LoggingService.Repositories;
using LoggingService.Services.RabbitMQ;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LoggingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Get the needed injected services
                    var logsRepository = services.GetRequiredService<ILogRepository>();
                    var rabbitMQConnection = services.GetRequiredService<IRabbitMQConnection>();


                    // RabbitMQ subcribe
                    RabbitSubscribedEvents.SubscribeRabbitMQEvents(rabbitMQConnection, logsRepository);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            host.Run();
        }

        public static IHost CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build();

        
    }
}

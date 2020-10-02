using UserService.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UserService.Services.RabbitMQ;
using RabbitMQ.Client.Events;
using Utils.Enums;
using System.Text;

namespace UserService
{
    public class Program
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {

            Console.Title = "JP Project - Api Management";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"jpProject_sso_log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5)
                .WriteTo.Console()
                .CreateLogger();

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                Task.WaitAll(DatabaseChecker.EnsureDatabaseIsReady(scope));
                var services = scope.ServiceProvider;
                try
                {
                    // initialize RabbitMQ
                    var rabbitMQConnection = services.GetRequiredService<IRabbitMQConnection>();
                    
                    // RabbitMQ subcribe
                    SubscribeRabbitMQEvents(rabbitMQConnection, services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while enabling RabbitMQ subscribed events.");
                }
            }
            host.Run();
        }

        public static IHost BuildWebHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddSerilog();
                    });
                    webBuilder.UseStartup<Startup>();
                }).Build();

        public static void SubscribeRabbitMQEvents(IRabbitMQConnection rabbitmqconn, IServiceProvider services)
        {
            #region Rabbit Event - consume UserApi Settings Data

            var consumerReceivedSettingsData = new EventingBasicConsumer(rabbitmqconn.UserChannel);

            consumerReceivedSettingsData.Received += (model, ea) =>
            {
                var body = ea.Body;

                //the setting queue message contains the setting type
                var message = Encoding.UTF8.GetString(body);

                int settingType = 0;
                int.TryParse(message, out settingType);

                if (settingType == (int)SettingsType.USER)
                {
                    // TODO (joaoOxOc): save user settings that comes from the Rabbit event
                    //bool settingsLoaded = LoadSettings(services);

                    //if (settingsLoaded)
                        rabbitmqconn.UserChannel.BasicAck(ea.DeliveryTag, false);
                }
            };

            rabbitmqconn.UserChannel.BasicConsume(RabbitQueues.UserSettingQ, false, RabbitTags.USERAPI, false, false, null, consumerReceivedSettingsData);

            #endregion
        }
    }
}

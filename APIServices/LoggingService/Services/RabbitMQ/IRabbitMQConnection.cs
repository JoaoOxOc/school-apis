using RabbitMQ.Client;

namespace LoggingService.Services.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        IModel ExchangeChannel { get; set; }

        IModel HangfireDeleteAccountChannel { get; set; }
    }
}

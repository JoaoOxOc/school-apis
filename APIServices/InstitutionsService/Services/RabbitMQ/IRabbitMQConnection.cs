using RabbitMQ.Client;

namespace InstitutionsService.Services.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        IModel LogsChannel { get; set; }
    }
}

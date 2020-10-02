using RabbitMQ.Client;
using Utils.Constants;

namespace InstitutionsService.Services.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        public IModel LogsChannel { get; set; }

        public RabbitMQConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = RabbitMQCredentials.Username;
            factory.Password = RabbitMQCredentials.Password;
            factory.VirtualHost = RabbitMQCredentials.VirtualHost;
            factory.HostName = RabbitMQCredentials.HostName;

            IConnection conn = factory.CreateConnection();
        }
    }
}
using RabbitMQ.Client;
using Utils.Constants;

namespace LoggingService.Services.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        public IModel ExchangeChannel { get; set; }
        public IModel HangfireDeleteAccountChannel { get; set; }

        public RabbitMQConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = RabbitMQCredentials.Username;
            factory.Password = RabbitMQCredentials.Password;
            factory.Port = RabbitMQCredentials.Port;
            factory.VirtualHost = RabbitMQCredentials.VirtualHost;
            factory.HostName = RabbitMQCredentials.HostName;

            IConnection conn = factory.CreateConnection();
            
            this.ExchangeChannel = conn.CreateModel();
            this.HangfireDeleteAccountChannel = conn.CreateModel();
        }

    }
}

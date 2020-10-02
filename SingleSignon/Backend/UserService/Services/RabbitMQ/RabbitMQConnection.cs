using RabbitMQ.Client;
using Utils.Constants;

namespace UserService.Services.RabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        public IModel UserChannel { get; set; }

        public IModel HangfireContractAboutToExpireChannel { get; set; }
        public IModel HangfireContractExpireChannel { get; set; }
        public IModel HangfireNewDiagnosisAlertChannel { get; set; }
        public IModel HangfireDeleteAccountChannel { get; set; }

        public RabbitMQConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = RabbitMQCredentials.Username;
            factory.Password = RabbitMQCredentials.Password;
            factory.VirtualHost = RabbitMQCredentials.VirtualHost;
            factory.HostName = RabbitMQCredentials.HostName;

            IConnection conn = factory.CreateConnection();

            this.UserChannel = conn.CreateModel();
            this.HangfireContractAboutToExpireChannel = conn.CreateModel();
            this.HangfireContractExpireChannel = conn.CreateModel();
            this.HangfireNewDiagnosisAlertChannel = conn.CreateModel();
            this.HangfireDeleteAccountChannel = conn.CreateModel();

            RegisterUserServiceExchange();
            
            //#region UserService Queue
            
            //var consumer = new EventingBasicConsumer(this.UserChannel);

            //consumer.Received += (model, ea) =>
            //{
            //    var body = ea.Body;
            //    var message = Encoding.UTF8.GetString(body);
            //};

            //this.UserChannel.BasicConsume("pep", true, consumer);
            
            //#endregion
        }

        #region Exchanges/Queues

        public void RegisterUserServiceExchange()
        {

            //string _exchangeName = "user.main";
            //string _queueName = "user-q.1";
            //string _type = ExchangeType.Fanout;
            //string _routingKey = "";

            //this.UserChannel.ExchangeDeclare(_exchangeName, _type);
            //this.UserChannel.QueueDeclare(_queueName, true, false, false, null);
            //this.UserChannel.QueueBind(_queueName, _exchangeName, _routingKey, null);
        }

        #endregion

    }
}

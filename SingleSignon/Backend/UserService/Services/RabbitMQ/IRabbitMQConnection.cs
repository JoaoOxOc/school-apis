using RabbitMQ.Client;

namespace UserService.Services.RabbitMQ
{
    public interface IRabbitMQConnection
    {
        IModel UserChannel { get; set; }

        IModel HangfireContractAboutToExpireChannel { get; set; }
        IModel HangfireContractExpireChannel { get; set; }
        IModel HangfireNewDiagnosisAlertChannel { get; set; }
        IModel HangfireDeleteAccountChannel { get; set; }
    }
}

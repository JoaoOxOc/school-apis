using LoggingService.Models;
using LoggingService.Repositories;
using LoggingService.Services.RabbitMQ;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;

namespace LoggingService.Code
{
    public class RabbitSubscribedEvents
    {
        public static void SubscribeRabbitMQEvents(IRabbitMQConnection rabbitmqconn, ILogRepository logRepository)
        {
            #region Rabbit Consume Logging Event

            RabbitConsumeLoggingEvent(rabbitmqconn, logRepository);

            #endregion

            #region Hangfire Delete Account

            //var consumerDeleteExpiredAccount = new EventingBasicConsumer(rabbitmqconn.HangfireDeleteAccountChannel);

            //consumerDeleteExpiredAccount.Received += (model, ea) =>
            //{
            //    var body = ea.Body;

            //    var message = Encoding.UTF8.GetString(body);
            //    HangfireDeleteExpiredAccountsRequest request = JsonConvert.DeserializeObject<HangfireDeleteExpiredAccountsRequest>(message);

            //    HangfireJobs.HangfireDeleteExpiredAccounts(logRepository, rabbitmqconn, request);

            //    rabbitmqconn.HangfireDeleteAccountChannel.BasicAck(ea.DeliveryTag, false);
            //};

            //rabbitmqconn.HangfireDeleteAccountChannel.BasicConsume(RabbitQueues.HangfireDeleteExpiredAccountsQ_Logs, false, "hangfire", false, false, null, consumerDeleteExpiredAccount);

            #endregion

        }

        private static void RabbitConsumeLoggingEvent(IRabbitMQConnection rabbitmqconn, ILogRepository logRepository)
        {
            var consumer = new EventingBasicConsumer(rabbitmqconn.ExchangeChannel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;

                var message = Encoding.UTF8.GetString(body);

                // Deserialize the log object

                Log log = JsonConvert.DeserializeObject<Log>(message);

                var saveTask = logRepository.Add(log);

                saveTask.ContinueWith((r) => {

                    if (r.IsCompletedSuccessfully)
                        rabbitmqconn.ExchangeChannel.BasicAck(ea.DeliveryTag, false);
                });

            };

            rabbitmqconn.ExchangeChannel.BasicConsume(RabbitQueues.LoggerApiQ, false, RabbitTags.LOGAPI, false, false, null, consumer);
        }
    }
}

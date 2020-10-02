using Entities.Hangfire.ViewModel;
using Entities.Log;
using LoggingService.Models;
using LoggingService.Repositories;
using LoggingService.Services.RabbitMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;
using Utils.Extensions;

namespace LoggingService.Code
{
    public static class HangfireJobs
    {
        public static bool HangfireDeleteExpiredAccounts(ILogRepository logsRepository, IRabbitMQConnection _rabbit, HangfireDeleteExpiredAccountsRequest request)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                int quantLogsDeleted = 0;

                foreach (int companyId in request.CompaniesIds)
                {
                    List<Log> _logs = logsRepository.GetFiltered(string.Empty, null, companyId.ToString(), null, null, null, null, null, string.Empty, "asc").ToList<Log>();
                    foreach(var companyLog in _logs)
                    {
                        if (companyLog.ActivityType != (int)LogActivityType.SystemActiviy)
                        {
                            logsRepository.Delete(companyLog.Id);
                            quantLogsDeleted++;
                        }
                    }
                }

                // finally, delete all DMS files
                foreach (int obj in request.DocIdsToDelete)
                {
                    string serializedVM = JsonConvert.SerializeObject(obj);
                    byte[] bytesVM = Encoding.UTF8.GetBytes(serializedVM);

                    _rabbit.ExchangeChannel.BasicPublish(RabbitExchanges.LOGS, RabbitEvents.DMSDelete, true, null, bytesVM);
                }

                watch.Stop();
                var miliseconds = watch.ElapsedMilliseconds;

                #region Log

                Log log = new Log();
                log.Date = DateTime.Now;
                log.Timezone = "UTC";
                log.EntityId = 0;
                log.EntityName = typeof(Log).Name;
                log.Type = (int)LogType.Debug;
                log.ActivityType = (int)LogActivityType.SystemActiviy;
                log.UserId = 0;
                log.Username = "Hangfire";
                log.CompanyId = string.Empty;

                log.Message = "HangfireDeleteExpiredAccounts-Logs executed in: " + miliseconds + " miliseconds // " + (miliseconds / 1000) + " seconds.";
                if (quantLogsDeleted > 0)
                {
                    log.Message += Environment.NewLine + quantLogsDeleted + " company logs deleted.";
                }
                else
                {
                    log.Message += Environment.NewLine + "No company log was deleted.";
                }

                logsRepository.Add(log);

                #endregion
            }
            catch (Exception ex)
            {
                Tuple<string, string> errorData = ExceptionHelper.GetLogMessageFromException(ex);

                Log log = new Log();
                log.CompanyId = null;
                log.Date = DateTime.Now;
                log.EntityId = null;
                log.EntityName = typeof(Log).Name;
                log.Message = errorData.Item2 + "\n" + errorData.Item1;
                log.Timezone = "UTC";
                log.UserId = 0;
                log.Username = "Hangfire";
                log.Type = (int)LogType.Error;
                log.ActivityType = (int)LogActivityType.ErrorAction;

                logsRepository.Add(log);
            }
            return true;
        }
    }
}

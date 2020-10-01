using Entities.Base;
using Entities.Log;
using Newtonsoft.Json;
using System;
using System.Text;
using Utils.Enums;
using Utils.Extensions;

namespace InstitutionsService.Code
{
    public static class ErrorLog
    {
        public static byte[] ErrorLogGenerator(Exception ex, int? entityId, string entityName, int userId, string userName, string companyId)
        {
            Tuple<string, string> errorData = ExceptionHelper.GetLogMessageFromException(ex);

            LogEvent log = new LogEvent();
            log.CompanyId = companyId;
            log.Date = DateTime.Now;
            log.EntityId = entityId;
            log.EntityName = entityName;
            log.Message = errorData.Item2 + "\n" + errorData.Item1;
            log.Timezone = "UTC";
            log.UserId = userId;
            log.Username = userName;
            log.Type = (int)LogType.Error;
            log.ActivityType = (int)LogActivityType.ErrorAction;

            string serializedLog = JsonConvert.SerializeObject(log);

            return Encoding.UTF8.GetBytes(serializedLog);
        }
    }
}

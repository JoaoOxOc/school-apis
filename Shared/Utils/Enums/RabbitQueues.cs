using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Enums
{
    public static class RabbitTags
    {
        public static string LOGAPI = "logAPI";

        public static string USERAPI = "userAPI";
    }

    public static class RabbitExchanges
    {
        public static string LOGS = "logs";
    }

    public static class RabbitQueues
    {
        public static string MailerQ = "mailer.queue";
        public static string LoggerApiQ = "logger.api.queue";

        public static string DmsSettingQ = "dmssetting.queue";
        public static string DmsCloneQ = "dms.clone.queue";
        public static string DmsDeleteQ = "dms.queue.delete";

        public static string EmailSettingQ = "emailsetting.queue";
        public static string RegSettingQ = "regsetting.queue";
        public static string RegCloneQ = "regulation.clone.queue";
        public static string UserSettingQ = "usersetting.queue";

        public static string HangfireContractToExpireQ = "hangfire.queue.contract-to-expire";
        public static string HangfireContractExpireQ = "hangfire.queue.contract-expire";
        public static string HangfireNewDiagnosisAlertQ_Regulation = "hangfire.queue.new-diagnosis-alert-regulation";
        public static string HangfireNewDiagnosisAlertQ_User = "hangfire.queue.new-diagnosis-alert-user";

        public static string RegulationNewUserStartupQ = "regulation.queue.new-user-startup";

        public static string HangfireDeleteExpiredAccountsQ_User = "hangfire.queue.delete-account-user";
        public static string HangfireDeleteExpiredAccountsQ_Regulation = "hangfire.queue.delete-account-regulation";
        public static string HangfireDeleteExpiredAccountsQ_Logs = "hangfire.queue.delete-account-logs";
        public static string NotifyQ = "notify.queue";
    }
    public static class RabbitEvents
    {
        public static string CreateLog = "log.event.create";
        public static string MailDispatch = "mail.event.dispatch";

        public static string DMSDelete = "dms.event.delete";

        public static string HangfireContractToExpire = "hangfire.event.contract-to-expire";
        public static string HangfireContractExpire = "hangfire.event.contract-expire";
        public static string HangfireNewDiagnosisAlert_Regulation = "hangfire.event.new-diagnosis-alert-regulation";
        public static string HangfireNewDiagnosisAlert_User = "hangfire.event.new-diagnosis-alert-user";

        public static string RegulationNewUserStartup = "regulation.event.new-user-startup";

        public static string HangfireDeleteExpiredAccounts_User = "hangfire.event.delete-account-user";
        public static string HangfireDeleteExpiredAccounts_Regulation = "hangfire.event.delete-account-regulation";
        public static string HangfireDeleteExpiredAccounts_Logs = "hangfire.event.delete-account-logs";
        public static string NotifyEvent = "notify.event.send";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Notification.ViewModel
{
    public class SysNotifyVM
    {
        public string SysNotify { get; set; }

        public List<string> UserNotifyProfiles { get; set; }

        public int? CompanyId { get; set; }

        public int? BusinessAreaId { get; set; }

        public int? ProcessId { get; set; }

        public string NotificationTemplateKey { get; set; }

        public KeyValuePair<int, string> UserFrom { get; set; }

        public List<KeyValuePair<int, string>> UserTo { get; set; }

        public List<Tuple<string, string>> TokensToReplace_Subject { get; set; }
        public List<Tuple<string, string>> TokensToReplace_Message { get; set; }
    }
}

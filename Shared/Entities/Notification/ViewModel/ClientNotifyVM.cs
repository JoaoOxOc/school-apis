using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Notification.ViewModel
{
    public class ClientNotifyVM
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}

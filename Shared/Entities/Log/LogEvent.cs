using System;

namespace Entities.Log
{
    public class LogEvent
    {
        public string Id { get; set; }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string CompanyId { get; set; }

        // Log Data
        public string Url { get; set; }
        public string Message { get; set; }
        public int Type { get; set; }
        public int? ActivityType { get; set; }

        // Changed entity
        public int? EntityId { get; set; }
        public string EntityName { get; set; }

        // Timestamps
        public DateTime Date { get; set; }

        public string Timezone { get; set; }
    }
}

using System;

namespace TapSync.Models
{
    public class AccessLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EventId { get; set; } = string.Empty;
        public string AttendeeId { get; set; } = string.Empty;
        public string Gate { get; set; } = string.Empty;
        public DateTime AccessTime { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Allowed"; // Allowed, Denied, OverrideAllowed
        public string Reason { get; set; } = string.Empty;
        public string? OperatorId { get; set; }
    }
}

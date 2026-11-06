using System;

namespace TapSync.Models
{
    public class AccessRule
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EventId { get; set; } = string.Empty;
        public string AttendeeCategory { get; set; } = string.Empty; // General, VIP, Staff, Press
        public string Gate { get; set; } = string.Empty;
        public bool IsAllowed { get; set; } = true;
        public string? Reason { get; set; }
    }
}

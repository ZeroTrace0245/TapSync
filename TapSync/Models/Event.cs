using System;

namespace TapSync.Models
{
    public class Event
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Venue { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalCapacity { get; set; }
        public string Status { get; set; } = "Planning"; // Planning, Active, Completed, Archived
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

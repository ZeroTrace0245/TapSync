using System;

namespace TapSync.Models
{
    public class Attendee
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EventId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Category { get; set; } = "General"; // General, VIP, Staff, Press
        public string CheckInStatus { get; set; } = "Registered"; // Registered, CheckedIn, Departed
        public DateTime? CheckInTime { get; set; }
        public string? CredentialId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string FullName => $"{FirstName} {LastName}";
    }
}

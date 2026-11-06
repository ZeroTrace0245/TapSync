using System;

namespace TapSync.Models
{
    public class Credential
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AttendeeId { get; set; } = string.Empty;
        public string CredentialCode { get; set; } = string.Empty; // NFC tag ID
        public string Status { get; set; } = "Active"; // Active, Revoked, Replaced
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public string? RevocationReason { get; set; }
    }
}

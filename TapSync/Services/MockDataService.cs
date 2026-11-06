using System;
using System.Collections.Generic;
using System.Linq;
using TapSync.Models;

namespace TapSync.Services
{
    public class MockDataService
    {
        private static Event? _currentEvent;
        private static readonly List<Attendee> _attendees = new();
        private static readonly List<Credential> _credentials = new();
        private static readonly List<AccessLog> _accessLogs = new();

        public MockDataService()
        {
            InitializeMockData();
        }

        private void InitializeMockData()
        {
            // Create a mock event
            _currentEvent = new Event
            {
                Id = "event-001",
                Name = "Tech Conference 2026",
                Venue = "Convention Center Hall A",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(7),
                TotalCapacity = 500,
                Status = "Planning"
            };

            // Create mock attendees
            var attendeeData = new[]
            {
                ("John", "Smith", "john.smith@example.com", "General"),
                ("Sarah", "Johnson", "sarah.johnson@example.com", "VIP"),
                ("Mike", "Williams", "mike.williams@example.com", "Staff"),
                ("Emily", "Brown", "emily.brown@example.com", "General"),
                ("David", "Davis", "david.davis@example.com", "Press"),
                ("Lisa", "Miller", "lisa.miller@example.com", "VIP"),
                ("James", "Wilson", "james.wilson@example.com", "General"),
                ("Rachel", "Moore", "rachel.moore@example.com", "Staff"),
                ("Chris", "Taylor", "chris.taylor@example.com", "General"),
                ("Amanda", "Anderson", "amanda.anderson@example.com", "General"),
            };

            foreach (var (first, last, email, category) in attendeeData)
            {
                var attendee = new Attendee
                {
                    Id = $"att-{Guid.NewGuid().ToString().Substring(0, 8)}",
                    EventId = _currentEvent.Id,
                    FirstName = first,
                    LastName = last,
                    Email = email,
                    Category = category,
                    CheckInStatus = Random.Shared.Next(0, 3) switch
                    {
                        0 => "Registered",
                        1 => "CheckedIn",
                        _ => "Departed"
                    },
                    CheckInTime = Random.Shared.Next(0, 2) == 1 ? 
                        DateTime.UtcNow.AddHours(-Random.Shared.Next(1, 12)) : null
                };
                _attendees.Add(attendee);
            }

            // Create credentials for attendees
            foreach (var attendee in _attendees)
            {
                var credentialCode = $"NFC-{Guid.NewGuid().ToString().Substring(0, 12).ToUpper()}";
                var credential = new Credential
                {
                    Id = $"cred-{Guid.NewGuid().ToString().Substring(0, 8)}",
                    AttendeeId = attendee.Id,
                    CredentialCode = credentialCode,
                    Status = Random.Shared.Next(0, 10) == 0 ? "Revoked" : "Active"
                };
                _credentials.Add(credential);
                attendee.CredentialId = credential.Id;
            }

            // Create mock access logs
            var gates = new[] { "Main Entrance", "VIP Entrance", "Staff Entrance", "Emergency Exit" };
            foreach (var attendee in _attendees.Where(a => a.CheckInStatus == "CheckedIn"))
            {
                var accessLog = new AccessLog
                {
                    Id = $"log-{Guid.NewGuid().ToString().Substring(0, 8)}",
                    EventId = _currentEvent.Id,
                    AttendeeId = attendee.Id,
                    Gate = gates[Random.Shared.Next(gates.Length)],
                    AccessTime = attendee.CheckInTime ?? DateTime.UtcNow,
                    Status = "Allowed",
                    Reason = "Access granted"
                };
                _accessLogs.Add(accessLog);
            }
        }

        public Event GetCurrentEvent()
        {
            return _currentEvent ?? throw new InvalidOperationException("No event available");
        }

        public IEnumerable<Attendee> GetAttendees(string eventId)
        {
            return _attendees.Where(a => a.EventId == eventId).ToList();
        }

        public Attendee? GetAttendee(string attendeeId)
        {
            return _attendees.FirstOrDefault(a => a.Id == attendeeId);
        }

        public IEnumerable<Credential> GetCredentials(string eventId)
        {
            var attendeeIds = _attendees.Where(a => a.EventId == eventId).Select(a => a.Id).ToList();
            return _credentials.Where(c => attendeeIds.Contains(c.AttendeeId)).ToList();
        }

        public Credential? GetCredentialByCode(string credentialCode)
        {
            return _credentials.FirstOrDefault(c => c.CredentialCode == credentialCode);
        }

        public IEnumerable<AccessLog> GetAccessLogs(string eventId)
        {
            return _accessLogs.Where(l => l.EventId == eventId).ToList();
        }

        public IEnumerable<AccessLog> GetAccessLogsByAttendee(string attendeeId)
        {
            return _accessLogs.Where(l => l.AttendeeId == attendeeId).ToList();
        }

        public int GetCheckInCount(string eventId)
        {
            return _attendees.Count(a => a.EventId == eventId && a.CheckInStatus == "CheckedIn");
        }

        public int GetTotalAttendees(string eventId)
        {
            return _attendees.Count(a => a.EventId == eventId);
        }

        public Dictionary<string, int> GetCheckInsByCategory(string eventId)
        {
            var categories = new[] { "General", "VIP", "Staff", "Press" };
            var result = new Dictionary<string, int>();

            foreach (var category in categories)
            {
                result[category] = _attendees.Count(a => 
                    a.EventId == eventId && 
                    a.Category == category && 
                    a.CheckInStatus == "CheckedIn");
            }

            return result;
        }

        public void AddAccessLog(AccessLog log)
        {
            _accessLogs.Add(log);
        }

        public void UpdateAttendeeCheckIn(string attendeeId, string status)
        {
            var attendee = _attendees.FirstOrDefault(a => a.Id == attendeeId);
            if (attendee != null)
            {
                attendee.CheckInStatus = status;
                if (status == "CheckedIn")
                {
                    attendee.CheckInTime = DateTime.UtcNow;
                }
            }
        }
    }
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using TapSync.Services;

namespace TapSync.Views
{
    public sealed partial class DashboardPage : Page
    {
        private MockDataService? _dataService;

        public DashboardPage()
        {
            InitializeComponent();
            Loaded += DashboardPage_Loaded;
        }

        private void DashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            _dataService = new MockDataService();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            if (_dataService == null) return;

            var currentEvent = _dataService.GetCurrentEvent();
            var attendees = _dataService.GetAttendees(currentEvent.Id).ToList();
            var credentials = _dataService.GetCredentials(currentEvent.Id).ToList();
            var accessLogs = _dataService.GetAccessLogs(currentEvent.Id).ToList();

            // Update event name
            EventNameBlock.Text = $"{currentEvent.Name} • {currentEvent.Venue}";

            // Update metrics
            var totalAttendees = attendees.Count;
            var checkedIn = attendees.Count(a => a.CheckInStatus == "CheckedIn");
            var checkInRate = totalAttendees > 0 ? (checkedIn * 100) / totalAttendees : 0;
            var activeCredentials = credentials.Count(c => c.Status == "Active");

            TotalAttendeesBlock.Text = totalAttendees.ToString();
            CheckedInBlock.Text = checkedIn.ToString();
            CheckInRateBlock.Text = $"{checkInRate}%";
            ActiveCredentialsBlock.Text = activeCredentials.ToString();

            // Update check-in by category
            LoadCheckInByCategory(attendees, totalAttendees);

            // Update recent access activity
            LoadRecentAccessActivity(accessLogs, attendees);
        }

        private void LoadCheckInByCategory(List<Models.Attendee> attendees, int totalAttendees)
        {
            var categories = new[] { "General", "VIP", "Staff", "Press" };
            var categoryData = new List<dynamic>();

            foreach (var category in categories)
            {
                var categoryAttendees = attendees.Where(a => a.Category == category).ToList();
                var checkedIn = categoryAttendees.Count(a => a.CheckInStatus == "CheckedIn");
                var percentage = categoryAttendees.Count > 0 ? (checkedIn * 100) / categoryAttendees.Count : 0;

                categoryData.Add(new
                {
                    Category = category,
                    Count = checkedIn,
                    Total = categoryAttendees.Count,
                    Percentage = percentage
                });
            }

            CategoryCheckInList.ItemsSource = categoryData;
        }

        private void LoadRecentAccessActivity(List<Models.AccessLog> accessLogs, List<Models.Attendee> attendees)
        {
            var attendeeMap = attendees.ToDictionary(a => a.Id, a => a.FullName);

            var recentActivity = accessLogs
                .OrderByDescending(l => l.AccessTime)
                .Take(10)
                .Select(log => new
                {
                    AttendeeId = log.AttendeeId,
                    AttendeeFullName = attendeeMap.ContainsKey(log.AttendeeId) 
                        ? attendeeMap[log.AttendeeId] 
                        : "Unknown",
                    Gate = log.Gate,
                    AccessTime = log.AccessTime,
                    AccessTimeFormatted = log.AccessTime.ToString("g"),
                    Status = log.Status
                })
                .ToList();

            RecentAccessList.ItemsSource = recentActivity;
        }
    }
}

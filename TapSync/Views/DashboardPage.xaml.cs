using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using TapSync.Services;

namespace TapSync.Views
{
    public sealed class StatTile
    {
        public string Glyph { get; init; } = "";
        public string Label { get; init; } = "";
        public string Value { get; init; } = "";
        public string Detail { get; init; } = "";
    }

    public sealed class CategoryProgress
    {
        public string Name { get; init; } = "";
        public string Summary { get; init; } = "";
        public double Percent { get; init; }
        public Brush Brush { get; init; } = new SolidColorBrush();
    }

    public sealed class ActivityItem
    {
        public string Name { get; init; } = "";
        public string Initials { get; init; } = "";
        public string Gate { get; init; } = "";
        public string Time { get; init; } = "";
    }

    public sealed partial class DashboardPage : Page
    {
        private static readonly (string Name, string BrushKey)[] Categories =
        {
            ("General", "CategoryGeneralBrush"),
            ("VIP", "CategoryVipBrush"),
            ("Staff", "CategoryStaffBrush"),
            ("Press", "CategoryPressBrush"),
        };

        public DashboardPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var data = MockDataService.GetInstance();
            var ev = data.GetCurrentEvent();
            var attendees = data.GetAttendees(ev.Id).ToList();
            var credentials = data.GetCredentials(ev.Id).ToList();
            var logs = data.GetAccessLogs(ev.Id).ToList();

            int checkedIn = attendees.Count(a => a.CheckInStatus == "CheckedIn");
            int activeCredentials = credentials.Count(c => c.Status == "Active");
            int daysToGo = Math.Max(0, (ev.StartDate.Date - DateTime.Today).Days);

            GreetingText.Text = DateTime.Now.Hour switch
            {
                < 12 => "Good morning",
                < 18 => "Good afternoon",
                _ => "Good evening"
            };
            EventSubtitleText.Text = $"{ev.Name}  ·  {ev.Venue}  ·  {ev.StartDate:dddd d MMMM}";

            StatsRepeater.ItemsSource = new List<StatTile>
            {
                new() { Glyph = "", Label = "Checked in", Value = $"{checkedIn} / {attendees.Count}",
                        Detail = $"{Percent(checkedIn, attendees.Count):0}% of registered attendees" },
                new() { Glyph = "", Label = "Active credentials", Value = activeCredentials.ToString(),
                        Detail = $"{credentials.Count - activeCredentials} revoked" },
                new() { Glyph = "", Label = "Gate taps today", Value = logs.Count.ToString(),
                        Detail = $"{logs.Count(l => l.Status == "Allowed")} allowed" },
                new() { Glyph = "", Label = "Event starts", Value = daysToGo == 0 ? "Today" : $"{daysToGo} days",
                        Detail = $"Capacity {ev.TotalCapacity}" },
            };

            CategoryList.ItemsSource = Categories.Select(c =>
            {
                var group = attendees.Where(a => a.Category == c.Name).ToList();
                int inside = group.Count(a => a.CheckInStatus == "CheckedIn");
                return new CategoryProgress
                {
                    Name = c.Name,
                    Summary = $"{inside} of {group.Count}",
                    Percent = Percent(inside, group.Count),
                    Brush = (Brush)Application.Current.Resources[c.BrushKey]
                };
            }).ToList();

            var activity = logs
                .OrderByDescending(l => l.AccessTime)
                .Take(5)
                .Select(l =>
                {
                    var person = data.GetAttendee(l.AttendeeId);
                    return new ActivityItem
                    {
                        Name = person?.FullName ?? "Unknown attendee",
                        Initials = person == null ? "?" : $"{person.FirstName.FirstOrDefault()}{person.LastName.FirstOrDefault()}",
                        Gate = $"{l.Gate} · {l.Status}",
                        Time = l.AccessTime.ToLocalTime().ToString("t")
                    };
                })
                .ToList();

            ActivityList.ItemsSource = activity;
            NoActivityText.Visibility = activity.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private static double Percent(int part, int whole) => whole == 0 ? 0 : part * 100.0 / whole;
    }
}

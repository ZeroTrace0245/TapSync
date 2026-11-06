using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using TapSync.Services;

namespace TapSync.Views
{
    public sealed partial class AccessPage : Page
    {
        private MockDataService? _dataService;

        public AccessPage()
        {
            InitializeComponent();
            Loaded += AccessPage_Loaded;
        }

        private void AccessPage_Loaded(object sender, RoutedEventArgs e)
        {
            _dataService = new MockDataService();
            LoadAccessLogs();
        }

        private void LoadAccessLogs()
        {
            if (_dataService == null) return;

            var currentEvent = _dataService.GetCurrentEvent();
            var accessLogs = _dataService.GetAccessLogs(currentEvent.Id).ToList();
            var attendees = _dataService.GetAttendees(currentEvent.Id).ToList();
            var attendeeMap = attendees.ToDictionary(a => a.Id, a => a.FullName);

            var statusColorMap = new Dictionary<string, Brush>
            {
                { "Allowed", (Brush)Application.Current.Resources["SuccessFill"] ?? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 204, 0)) },
                { "Denied", (Brush)Application.Current.Resources["ErrorFill"] ?? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0)) },
                { "OverrideAllowed", (Brush)Application.Current.Resources["WarningFill"] ?? new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 192, 0)) }
            };

            var accessData = accessLogs
                .OrderByDescending(l => l.AccessTime)
                .Select(log => new
                {
                    AttendeeId = log.AttendeeId,
                    AttendeeFullName = attendeeMap.ContainsKey(log.AttendeeId) 
                        ? attendeeMap[log.AttendeeId] 
                        : "Unknown",
                    Gate = log.Gate,
                    AccessTime = log.AccessTime,
                    AccessTimeFormatted = log.AccessTime.ToString("g"),
                    Status = log.Status,
                    Reason = log.Reason,
                    StatusColor = statusColorMap.ContainsKey(log.Status) 
                        ? statusColorMap[log.Status] 
                        : new SolidColorBrush(Windows.UI.Color.FromArgb(255, 200, 200, 200))
                })
                .ToList();

            AccessLogsList.ItemsSource = accessData;
        }
    }
}

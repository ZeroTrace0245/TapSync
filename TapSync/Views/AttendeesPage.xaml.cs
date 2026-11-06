using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using TapSync.Services;

namespace TapSync.Views
{
    public sealed partial class AttendeesPage : Page
    {
        private MockDataService? _dataService;
        private List<dynamic> _allAttendees = new();

        public AttendeesPage()
        {
            InitializeComponent();
            Loaded += AttendeesPage_Loaded;
        }

        private void AttendeesPage_Loaded(object sender, RoutedEventArgs e)
        {
            _dataService = new MockDataService();
            LoadAttendees();
            SearchBox.TextChanged += SearchBox_TextChanged;
        }

        private void LoadAttendees()
        {
            if (_dataService == null) return;

            var currentEvent = _dataService.GetCurrentEvent();
            var attendees = _dataService.GetAttendees(currentEvent.Id).ToList();

            _allAttendees = attendees.Select(a => new
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                FullName = a.FullName,
                Email = a.Email,
                Category = a.Category,
                CheckInStatus = a.CheckInStatus,
                CheckInTime = a.CheckInTime,
                CheckInTimeFormatted = a.CheckInTime?.ToString("g") ?? "Not checked in",
                CredentialId = a.CredentialId
            }).Cast<dynamic>().ToList();

            AttendeesList.ItemsSource = _allAttendees;
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var query = sender.Text.ToLower();
            if (string.IsNullOrEmpty(query))
            {
                AttendeesList.ItemsSource = _allAttendees;
            }
            else
            {
                var filtered = _allAttendees
                    .Where(a => a.FullName.ToLower().Contains(query) || a.Email.ToLower().Contains(query))
                    .ToList();
                AttendeesList.ItemsSource = filtered;
            }
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string attendeeId)
            {
                _dataService?.UpdateAttendeeCheckIn(attendeeId, "CheckedIn");
                LoadAttendees();

                var dialog = new ContentDialog
                {
                    Title = "Check-in Successful",
                    Content = "Attendee has been checked in.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}
